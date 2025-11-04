using AutoMapper;
using movie_service_backend.DTO.CommentDTOs;
using movie_service_backend.DTO.RatingDTOs;
using movie_service_backend.Interfaces;
using movie_service_backend.Repo;

namespace movie_service_backend.Services
{
    public class CommentService : ICommentService
    {
        private readonly IMapper _mapper;
        private readonly CommentRepo _repo;

        public CommentService(IMapper mapper, CommentRepo repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<CommentDTO> CreateForFilmAsync(CommentCreateFilmDTO dto)
        {
            var comment = _mapper.Map<Comment>(dto);
            await _repo.AddAsync(comment);
            await _repo.SaveChangesAsync();
            return _mapper.Map<CommentDTO>(comment);
        }

        public async Task<CommentDTO> CreateForSeriesAsync(CommentCreateSeriesDTO dto)
        {
            var comment = _mapper.Map<Comment>(dto);
            await _repo.AddAsync(comment);
            await _repo.SaveChangesAsync();
            return _mapper.Map<CommentDTO>(comment);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var comment = await _repo.GetByIdAsync(id);
            if (comment == null) return false;
            _repo.Delete(comment);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CommentDTO>> GetAllAsync()
        {
            var comments = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<CommentDTO>>(comments);
        }

        public async Task<CommentDTO?> GetByIdAsync(int id)
        {
            var comment = await _repo.GetByIdAsync(id);
            return comment == null ? null : _mapper.Map<CommentDTO>(comment);
        }

        public async Task<CommentDTO> UpdateAsync(int id, CommentUpdateDTO dto)
        {
            var comment = await _repo.GetByIdAsync(id);
            if (comment == null)
                throw new Exception("Comment Not Found");
            comment.Text = dto.Text;
            _repo.Update(comment);
            await _repo.SaveChangesAsync();
            return _mapper.Map<CommentDTO>(comment);
        }
    }
}
