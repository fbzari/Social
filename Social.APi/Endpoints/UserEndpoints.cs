using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Social.APi.Dtos;
using Social.APi.Helpers;
using Social.APi.Models;
using Social.APi.Repository;
using Social.APi.Services;

namespace Social.APi.Endpoints
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api");

            group.MapPost("/signup", SignUpAsync)
                .WithName("SignUp")
                .Produces<ResponseDTO<UserDTO>>(StatusCodes.Status201Created)
                .Produces<ResponseDTO<object>>(StatusCodes.Status400BadRequest);

            group.MapPost("/login", LoginAsync)
                .WithName("Login")
                .Produces<ResponseDTO<string>>(StatusCodes.Status200OK)
                .Produces<ResponseDTO<object>>(StatusCodes.Status400BadRequest)
                .Produces<ResponseDTO<object>>(StatusCodes.Status401Unauthorized);

            group.MapGet("/users", GetUserProfileAsync)
                .WithName("AllUsers")
                .Produces<ResponseDTO<IEnumerable<UserDTO>>>(StatusCodes.Status200OK)
                .RequireAuthorization();
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

                return Results.BadRequest(new ResponseDTO<UserSignUpDTO>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = string.Join(";", validationResult.Errors.Select(e => e.ErrorMessage))

                });
            }

            if (!await authService.IsEmailUniqueAsync(userSignUpDto.Email)) {

                return Results.BadRequest(new ResponseDTO<UserSignUpDTO>
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

            return Results.CreatedAtRoute("Login", new ResponseDTO<UserDTO>
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

                return Results.BadRequest(new ResponseDTO<UserSignUpDTO>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = string.Join(";", validationResult.Errors.Select(e => e.ErrorMessage))

                });
            }

            var user = await authService.GetUserByEmailAsync(userLoginDTO.Email);

            if (user is null || !PasswordHasher.VerifyPassword(userLoginDTO.Password,user.PasswordHash)) {

                return Results.Json(
                    new ResponseDTO<string>
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status401Unauthorized,
                        ErrorMessage = "Invalid email or password."
                    }
                );
            }

            var token = tokenProvider.create(user);

            return Results.Ok(new ResponseDTO<string>
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

            return Results.Ok(new ResponseDTO<IEnumerable<UserDTO>>
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                Result = userDtos
            });

        }
    }
}
