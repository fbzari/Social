using AutoMapper;
using Social.APi.Models;

namespace Social.APi.Dtos
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapping for basic User to UserDTO
            CreateMap<User, UserDTO>().ReverseMap();

            CreateMap<UserSignUpDTO, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) 
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
                .ForMember(dest => dest.SentFriendRequests, opt => opt.Ignore())
                .ForMember(dest => dest.ReceivedFriendRequests, opt => opt.Ignore())
                .ForMember(dest => dest.Friendships, opt => opt.Ignore());

            CreateMap<UserLoginDTO, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) 
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
                .ForMember(dest => dest.SentFriendRequests, opt => opt.Ignore())
                .ForMember(dest => dest.ReceivedFriendRequests, opt => opt.Ignore())
                .ForMember(dest => dest.Friendships, opt => opt.Ignore());
        }
    }
}
