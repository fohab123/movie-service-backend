using AutoMapper;
using movie_service_backend.DTO.RatingDTOs;
using movie_service_backend.Models;

namespace movie_service_backend.Mapping
{
    public class RatingProfile : Profile
    {
        public RatingProfile()
        {
            CreateMap<Rating,RatingDTO>().ReverseMap();
            CreateMap<RatingCreateFilmDTO,Rating>().ReverseMap();
            CreateMap<RatingCreateSeriesDTO, Rating>().ReverseMap();
            CreateMap<RatingUpdateDTO,Rating>().ReverseMap();
        }  
    }
}
