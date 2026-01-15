using AutoMapper;
using movie_service_backend.DTO.DebatePostDTOs;
using movie_service_backend.Models;

namespace movie_service_backend.Mapping
{
    public class DebateProfile : Profile
    {
        public DebateProfile()
        {
            CreateMap<DebatePostCreateDTO, DebatePost>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Parent, opt => opt.Ignore())
            .ForMember(dest => dest.Replies, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<DebatePost, DebatePostDTO>()
            .ForMember(dest => dest.Username,
                opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.Replies,
                opt => opt.MapFrom(src => src.Replies))
            .ForMember(dest => dest.LikesCount,
                opt => opt.MapFrom(src => src.Likes.Count)
            );
            CreateMap<DebatePost, DebatePostLikesDTO>()
            .ForMember(d => d.Username,
                o => o.MapFrom(s => s.User.Username))
            .ForMember(d => d.LikesCount,
                o => o.MapFrom(s => s.Likes.Count));

        }
    }
}
