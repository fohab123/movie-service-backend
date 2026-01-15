using movie_service_backend.Interfaces;
using movie_service_backend.DTO.DebatePostDTOs;
using movie_service_backend.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using movie_service_backend.Repo;

namespace movie_service_backend.Services
{
    public class DebateService : IDebateService
    {
        private readonly DebatePostLikeRepo _likeRepo;
        private readonly IMapper _mapper;
        private readonly DebateRepo _repo;
        public DebateService(IMapper mapper, DebatePostLikeRepo likeRepo, DebateRepo repo)
        {
            _mapper = mapper;
            _repo = repo;
            _likeRepo = likeRepo;
        }

        public async Task<DebatePostDTO> CreatePostAsync(DebatePostCreateDTO dto, int userId)
        {
            if (dto.ParentId.HasValue)
            {
                var parent = await _repo.GetByIdAsync(dto.ParentId.Value);
                if (parent == null)
                    throw new Exception("Parent debate post does not exist");
            }

            var post = new DebatePost
            {
                Title = dto.ParentId == null ? dto.Title : null, // title samo root
                Content = dto.Content,
                ParentId = dto.ParentId, // NULL ili validan ID
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(post);
            await _repo.SaveChangesAsync();

            return _mapper.Map<DebatePostDTO>(post);
        }

        private async Task DeleteRecursive(DebatePost post)
        {
            foreach (var reply in post.Replies.ToList())
                await DeleteRecursive(reply);

            _repo.Delete(post);
        }

        public async Task<bool> DeletePostAsync(int postId)
        {
            var post = await _repo.GetByIdAsync(postId);
            if (post == null) return false;

            await DeleteRecursive(post);

            return await _repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<DebatePostDTO>> GetAllRootPostsAsync()
        {
            var posts = await _repo.GetAllAsync();
            var roots = posts.Where(p => p.ParentId == null);
            return _mapper.Map<IEnumerable<DebatePostDTO>>(roots);
        }

        public async Task<DebatePostDTO?> GetDebateThreadAsync(int postId)
        {
            var post = await _repo.GetByIdAsync(postId);
            if (post == null) return null;
            return _mapper.Map<DebatePostDTO>(post);
        }
        public async Task ToggleLikeAsync(int userId, int postId)
        {
            // 1. Proveri da li post postoji
            var post = await _repo.GetByIdAsync(postId);
            if (post == null)
                throw new Exception("Debate post does not exist");

            // 2. Proveri da li već postoji like
            var existing = await _likeRepo.GetByPostAndUserAsync(postId, userId);

            if (existing != null)
            {
                _likeRepo.Delete(existing);
            }
            else
            {
                var like = new DebatePostLike
                {
                    DebatePostId = postId,
                    UserId = userId
                };
                await _likeRepo.AddAsync(like);
            }

            await _likeRepo.SaveChangesAsync();
        }

        public async Task<int> GetLikesCountAsync(int postId)
        {
            return await _likeRepo.CountByPostAsync(postId);
        }
    }
}
