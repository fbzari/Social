using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Social.APi.Dtos;
using Social.APi.Extensions;
using Social.APi.Helpers;
using Social.APi.Models;
using Social.APi.Repository;
using Social.APi.Services;
using System.Security.Claims;

namespace Social.APi.Endpoints
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder app)
        {

            app.MapPost("/signup", SignUpAsync).WithName("SignUp");
            app.MapPost("/login", LoginAsync).WithName("Login");

            var group = app.MapGroup("/api/users").RequireAuthorization();

            group.MapGet("", GetUserProfileAsync).WithName("AllUsers");
            group.MapPost("/send-request", SendFriendRequestAsync).WithName("SendFriendRequest");
            group.MapGet("/view-request", ViewFriendAsync).WithName("ViewFriendRequest");

        }

        private static async Task<IResult> SignUpAsync(

            [FromBody] UserSignUpDTO userSignUpDto,
            IValidator<UserSignUpDTO> validator,
            IAuthService authService,
            IMapper mapper
            )
        {
            var validationResult = await validator.ValidateAsync(userSignUpDto);

            if (!validationResult.IsValid) {

                return TypedResults.BadRequest(new ResponseDTO<UserSignUpDTO>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = string.Join(";", validationResult.Errors.Select(e => e.ErrorMessage))

                });
            }

            if (!await authService.IsEmailUniqueAsync(userSignUpDto.Email)) {

                return TypedResults.BadRequest(new ResponseDTO<UserSignUpDTO>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Email Id should be unique !"
                });
            }
            
            var user = mapper.Map<User>(userSignUpDto);
            user.PasswordHash = PasswordHasher.Hash(userSignUpDto.Password);

            await authService.CreateUserAsync(user);
            var userDto = mapper.Map<UserDTO>(user);

            return TypedResults.CreatedAtRoute("Login", new ResponseDTO<UserDTO>
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status201Created,
                Result = userDto
            });


        }

        private static async Task<IResult> LoginAsync(
            [FromBody] UserLoginDTO userLoginDTO,
            IValidator<UserLoginDTO> validator,
            IAuthService authService,
            IMapper mapper,
            TokenProvider tokenProvider
            )
        {
            var validationResult = await validator.ValidateAsync(userLoginDTO);

            if (!validationResult.IsValid) {

                return TypedResults.BadRequest(new ResponseDTO<UserSignUpDTO>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = string.Join(";", validationResult.Errors.Select(e => e.ErrorMessage))

                });
            }

            var user = await authService.GetUserByEmailAsync(userLoginDTO.Email);

            if (user is null || !PasswordHasher.VerifyPassword(userLoginDTO.Password,user.PasswordHash)) {

                return TypedResults.Json(
                    new ResponseDTO<string>
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status401Unauthorized,
                        ErrorMessage = "Invalid email or password."
                    }
                );
            }

            var token = tokenProvider.create(user);

            return TypedResults.Ok(new ResponseDTO<string>
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                Result = token
            });

        }

        // Get user profile endpoint
        private static async Task<IResult> GetUserProfileAsync(
            IAuthService authService,
            IMapper mapper
        )
        {

            var UserList = await authService.GetAllUsers();

            var userDtos = mapper.Map<IEnumerable<UserDTO>>(UserList);

            return TypedResults.Ok(new ResponseDTO<IEnumerable<UserDTO>>
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                Result = userDtos
            });

        }

        private static async Task<IResult> SendFriendRequestAsync(
            [FromBody] FriendRequestDto friendRequestDto,
            IAuthService authService,
            IFriendRequestRepository friend,
            IValidator<FriendRequestDto> validator,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor
            )
        {
            #region Vadidate - Edge case
            var validationResult = await validator.ValidateAsync(friendRequestDto);
            if (!validationResult.IsValid)
            {
                return TypedResults.BadRequest(new ResponseDTO<FriendRequestDto>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))
                });
            }

            var senderEmail = Helper.GetSenderEmailFromJwt(httpContextAccessor.HttpContext);

            if (string.IsNullOrEmpty(senderEmail))
            {
                return TypedResults.Json(new ResponseDTO<FriendRequestDto>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status401Unauthorized,
                    ErrorMessage = "Sender email is empty. I think you are unauthorized"
                });
            }

            // Fetch sender and receiver from the database
            var sender = await authService.GetUserByEmailAsync(senderEmail);
            var receiver = await authService.GetUserByEmailAsync(friendRequestDto.ReceiverId);

            if (sender is null || receiver is null)
            {
                return TypedResults.BadRequest(new ResponseDTO<FriendRequestDto>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Sender or receiver not found."
                });
            }

            if (sender.Email == receiver.Email)
            {
                return TypedResults.BadRequest(new ResponseDTO<FriendRequestDto>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Cannot send a friend request to yourself."
                });
            }


            var existingRequest = await friend.GetRequestAsync(senderEmail, friendRequestDto.ReceiverId);

            if (existingRequest != null)
            {
                return TypedResults.BadRequest(new ResponseDTO<FriendRequestDto>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "A friend request already exists between these users."
                });
            }

            #endregion

            var friendRequest = new FriendRequest
            {
                SentAt = DateTime.UtcNow,
                Status = nameof(UserActions.Pending),
                SenderId = senderEmail,
                ReceiverId = friendRequestDto.ReceiverId
            };

            await friend.AddAsync(friendRequest);

            return TypedResults.Ok( new ResponseDTO<string>
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                Result = "Friend Request send successfully!!!"
            });


        }

        private static async Task<IResult> ViewFriendAsync(
            IFriendService friend,
            IHttpContextAccessor http,
            IMapper mapper
            )
        {
            var email = Helper.GetSenderEmailFromJwt(http.HttpContext);

            if (string.IsNullOrEmpty(email))
            {

                return TypedResults.BadRequest(new ResponseDTO<FriendRequestDto>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "There is error while getting user login ID. logout and try again"
                });

            }

            var frienResponse = await friend.GetFrienrequestAsync(email);

            var result = mapper.Map<IEnumerable<FriendRequestRespondDTO>>(frienResponse);

            if (!result.Any()) {

                return TypedResults.NotFound(new ResponseDTO<string>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorMessage = "You dont have any friend requests"
                });
            }

            return TypedResults.Ok(new ResponseDTO<IEnumerable<FriendRequestRespondDTO>>
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                Result = result
            });
        }

    }
}
