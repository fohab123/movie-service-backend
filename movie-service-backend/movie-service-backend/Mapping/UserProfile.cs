using AutoMapper;
using movie_service_backend.DTO.UserDTOs;
using movie_service_backend.Models;

namespace movie_service_backend.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile() {

            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserAdminDTO>().ReverseMap();
            CreateMap<UserCreateDTO, User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore());
        }
    }
}
