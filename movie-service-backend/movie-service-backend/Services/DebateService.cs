using movie_service_backend.Interfaces;
using movie_service_backend.DTO.DebatePostDTOs;
using movie_service_backend.Models;
using AutoMapper;
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
                Title = dto.ParentId == null ? dto.Title : null,
                Content = dto.Content,
                ParentId = dto.ParentId,
                FilmId = dto.ParentId == null ? dto.FilmId : null,
                SeriesId = dto.ParentId == null ? dto.SeriesId : null,
                Tags = dto.ParentId == null && dto.Tags.Any() ? string.Join(",", dto.Tags) : null,
                IsSpoiler = dto.ParentId == null && dto.IsSpoiler,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(post);
            await _repo.SaveChangesAsync();

            // Re-fetch to get User navigation property for mapping
            var saved = await _repo.GetByIdAsync(post.Id);
            return MapToDto(saved!, userId);
        }

        public async Task<IEnumerable<DebatePostDTO>> GetAllAsync(string? sort, int? filmId, int? seriesId, int? userId)
        {
            var posts = await _repo.GetRootPostsFilteredAsync(filmId, seriesId);

            var sorted = sort switch
            {
                "mostliked" => posts.OrderByDescending(p => p.Likes.Count),
                "mostcommented" => posts.OrderByDescending(p => p.Replies.Count),
                "trending" => posts
                    .Where(p => p.CreatedAt >= DateTime.UtcNow.AddDays(-7))
                    .OrderByDescending(p => p.Likes.Count + p.Replies.Count)
                    .AsEnumerable()
                    .Concat(posts
                        .Where(p => p.CreatedAt < DateTime.UtcNow.AddDays(-7))
                        .OrderByDescending(p => p.CreatedAt)),
                _ => posts.OrderByDescending(p => p.CreatedAt) // "newest" default
            };

            return sorted.Select(p => MapToDto(p, userId)).ToList();
        }

        public async Task<IEnumerable<DebatePostDTO>> GetAllRootPostsAsync()
        {
            return await GetAllAsync(null, null, null, null);
        }

        public async Task<DebatePostDTO?> GetDebateThreadAsync(int postId, int? userId)
        {
            var post = await _repo.GetByIdAsync(postId);
            if (post == null) return null;
            return MapToDto(post, userId);
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

        public async Task ToggleLikeAsync(int userId, int postId)
        {
            var post = await _repo.GetByIdAsync(postId);
            if (post == null)
                throw new Exception("Debate post does not exist");

            var existing = await _likeRepo.GetByPostAndUserAsync(postId, userId);
            if (existing != null)
                _likeRepo.Delete(existing);
            else
                await _likeRepo.AddAsync(new DebatePostLike { DebatePostId = postId, UserId = userId });

            await _likeRepo.SaveChangesAsync();
        }

        public async Task<int> GetLikesCountAsync(int postId)
        {
            return await _likeRepo.CountByPostAsync(postId);
        }

        public async Task IncrementViewAsync(int postId)
        {
            var post = await _repo.GetByIdAsync(postId);
            if (post == null) return;
            post.ViewCount++;
            _repo.Update(post);
            await _repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<DebatePostDTO>> GetByFilmAsync(int filmId, int? userId)
        {
            var posts = await _repo.GetByFilmIdAsync(filmId);
            return posts.Select(p => MapToDto(p, userId)).ToList();
        }

        public async Task<IEnumerable<DebatePostDTO>> GetBySeriesAsync(int seriesId, int? userId)
        {
            var posts = await _repo.GetBySeriesIdAsync(seriesId);
            return posts.Select(p => MapToDto(p, userId)).ToList();
        }

        private DebatePostDTO MapToDto(DebatePost post, int? userId)
        {
            var dto = new DebatePostDTO
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                ParentId = post.ParentId,
                UserId = post.UserId,
                Username = post.User?.Username,
                FilmId = post.FilmId,
                FilmTitle = post.Film?.Title,
                FilmPosterUrl = post.Film?.PosterUrl,
                SeriesId = post.SeriesId,
                SeriesTitle = post.Series?.Title,
                SeriesPosterUrl = post.Series?.PosterUrl,
                Tags = string.IsNullOrEmpty(post.Tags)
                    ? new List<string>()
                    : post.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
                IsSpoiler = post.IsSpoiler,
                ViewCount = post.ViewCount,
                LikesCount = post.Likes?.Count ?? 0,
                ReplyCount = post.Replies?.Count ?? 0,
                IsLikedByUser = userId.HasValue && (post.Likes?.Any(l => l.UserId == userId.Value) ?? false),
                Replies = post.Replies?
                    .OrderByDescending(r => r.Likes?.Count ?? 0)
                    .ThenByDescending(r => r.CreatedAt)
                    .Select(r => MapToDto(r, userId))
                    .ToList() ?? new List<DebatePostDTO>()
            };
            return dto;
        }
    }
}
