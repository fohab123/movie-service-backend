using AutoMapper;
using movie_service_backend.DTO.FilmDTOs;
using movie_service_backend.Models;

namespace movie_service_backend.Mapping
{
    public class FilmProfile : Profile
    {
        public FilmProfile()
        {
            CreateMap<Film, FilmDTO>();
            CreateMap<Genre, GenreDTO>();
            CreateMap<FilmCreateDTO, Film>()
    .ForMember(dest => dest.Genre, opt => opt.Ignore());

        }
    }
}
