using AutoMapper;
using movie_service_backend.DTO.CommentDTOs;

namespace movie_service_backend.Mapping
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CommentDTO>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.User != null ? s.User.Username : string.Empty));
            CreateMap<CommentDTO, Comment>();
            CreateMap<CommentCreateFilmDTO, Comment>().ReverseMap();
            CreateMap<CommentCreateSeriesDTO, Comment>().ReverseMap();
            CreateMap<Comment, CommentUpdateDTO>().ReverseMap();
                
        }
    }
}
