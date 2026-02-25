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
            var existing = await _repo.GetUserFilmRatingAsync(dto.UserId, dto.FilmId);
            if (existing != null)
                return null;

            var rating = _mapper.Map<Rating>(dto);

            await _repo.AddAsync(rating);
            await _repo.SaveChangesAsync();

            return _mapper.Map<RatingDTO>(rating);
        }
        public async Task<RatingDTO> CreateForSeriesAsync(RatingCreateSeriesDTO dto)
        {
            var existing = await _repo.GetUserSeriesRatingAsync(dto.UserId, dto.SeriesId);
            if (existing != null)
                return null;

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

        public async Task<IEnumerable<RatingDTO>> GetByFilmIdAsync(int filmId)
        {
            var ratings = await _repo.GetByFilmIdAsync(filmId);
            return _mapper.Map<IEnumerable<RatingDTO>>(ratings);
        }

        public async Task<IEnumerable<RatingDTO>> GetBySeriesIdAsync(int seriesId)
        {
            var ratings = await _repo.GetBySeriesIdAsync(seriesId);
            return _mapper.Map<IEnumerable<RatingDTO>>(ratings);
        }

        public async Task<RatingDTO?> GetUserFilmRatingAsync(int userId, int filmId)
        {
            var rating = await _repo.GetUserFilmRatingAsync(userId, filmId);
            return rating == null ? null : _mapper.Map<RatingDTO>(rating);
        }

        public async Task<RatingDTO?> GetUserSeriesRatingAsync(int userId, int seriesId)
        {
            var rating = await _repo.GetUserSeriesRatingAsync(userId, seriesId);
            return rating == null ? null : _mapper.Map<RatingDTO>(rating);
        }
    }
}
