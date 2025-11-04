using AutoMapper;
using movie_service_backend.DTO.FilmDTOs;
using movie_service_backend.DTO.SeriesDTOs;
using movie_service_backend.Models;

namespace movie_service_backend.Mapping
{
    public class SeriesProfile : Profile
    {
        public SeriesProfile()
        {
            CreateMap<Series, SeriesDTO>();
            CreateMap<Genre, GenreDTO>();
            CreateMap<SeriesCreateDTO, Series>()
                .ForMember(dest => dest.Genre, opt => opt.Ignore());
            CreateMap<SeriesUpdateDTO, Series>()
                .ForMember(dest => dest.Genre, opt => opt.Ignore());
        }
    }
}
