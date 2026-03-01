namespace movie_service_backend.DTO.DebatePostDTOs
{
    public class DebatePostDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ParentId { get; set; }

        public int UserId { get; set; }
        public string? Username { get; set; }

        public int? FilmId { get; set; }
        public string? FilmTitle { get; set; }
        public string? FilmPosterUrl { get; set; }

        public int? SeriesId { get; set; }
        public string? SeriesTitle { get; set; }
        public string? SeriesPosterUrl { get; set; }

        public List<string> Tags { get; set; } = new List<string>();
        public bool IsSpoiler { get; set; }
        public int ViewCount { get; set; }

        public int LikesCount { get; set; }
        public int ReplyCount { get; set; }
        public bool IsLikedByUser { get; set; }

        public List<DebatePostDTO> Replies { get; set; } = new List<DebatePostDTO>();
    }
}
