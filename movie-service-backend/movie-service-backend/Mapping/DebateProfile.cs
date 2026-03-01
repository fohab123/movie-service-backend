using AutoMapper;
using movie_service_backend.DTO.DebatePostDTOs;
using movie_service_backend.Models;

namespace movie_service_backend.Mapping
{
    // Note: DebatePost → DebatePostDTO mapping is handled manually in DebateService.MapToDto
    // to support IsLikedByUser (requires userId context) and recursive reply mapping.
    // This profile only covers the CreateDTO → Model mapping.
    public class DebateProfile : Profile
    {
        public DebateProfile()
        {
            CreateMap<DebatePostCreateDTO, DebatePost>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Parent, opt => opt.Ignore())
                .ForMember(dest => dest.Replies, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Film, opt => opt.Ignore())
                .ForMember(dest => dest.Series, opt => opt.Ignore())
                .ForMember(dest => dest.Likes, opt => opt.Ignore())
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src =>
                    src.Tags != null && src.Tags.Any()
                        ? string.Join(",", src.Tags)
                        : null));
        }
    }
}
