namespace movie_service_backend.DTO.ActivityDTOs
{
    public class RecentActivityDTO
    {
        public string Type { get; set; }           // "comment" | "rating"
        public string Username { get; set; }
        public int UserId { get; set; }
        public string MediaTitle { get; set; }
        public string MediaType { get; set; }      // "film" | "series"
        public int MediaId { get; set; }
        public string? MediaPosterUrl { get; set; }
        public string? Text { get; set; }          // comment text
        public int? Value { get; set; }            // rating value 1-10
        public DateTime CreatedAt { get; set; }
    }
}
