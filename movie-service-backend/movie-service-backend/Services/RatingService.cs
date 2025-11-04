using AutoMapper;
using movie_service_backend.DTO.RatingDTOs;
using movie_service_backend.Interfaces;
using movie_service_backend.Repo;

namespace movie_service_backend.Services
{
    public class RatingService : IRatingService
    {
        private readonly RatingRepo _repo;
        private readonly IMapper _mapper;

        public RatingService(RatingRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<RatingDTO> CreateForFilmAsync(RatingCreateFilmDTO dto)
        {
            var rating = _mapper.Map<Rating>(dto);
            await _repo.AddAsync(rating);
            await _repo.SaveChangesAsync();
            return _mapper.Map<RatingDTO>(rating);
        }
        public async Task<RatingDTO> CreateForSeriesAsync(RatingCreateSeriesDTO dto)
        {
            var rating = _mapper.Map<Rating>(dto);
            await _repo.AddAsync(rating);
            await _repo.SaveChangesAsync();
            return _mapper.Map<RatingDTO>(rating);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var rating = await _repo.GetByIdAsync(id);
            if(rating == null) return false;
            _repo.Delete(rating);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<RatingDTO>> GetAllAsync()
        {
            var ratings = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<RatingDTO>>(ratings);
        }

        public async Task<RatingDTO?> GetByIdAsync(int id)
        {
            var rating = await _repo.GetByIdAsync(id);
            return rating == null ? null : _mapper.Map<RatingDTO>(rating);
        }

        public async Task<RatingDTO> UpdateAsync(int id, RatingUpdateDTO dto)
        {
            var rating = await _repo.GetByIdAsync(id);
            if (rating == null)
                throw new Exception("Rating Not Found");
            rating.Value = dto.Value;
            _repo.Update(rating);
            await _repo.SaveChangesAsync();
            return _mapper.Map<RatingDTO>(rating);
        }
    }
}
