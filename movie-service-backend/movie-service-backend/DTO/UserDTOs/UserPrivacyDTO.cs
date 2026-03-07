namespace movie_service_backend.DTO.UserDTOs
{
    public class UserPrivacyDTO
    {
        public string CommentsVisibility { get; set; } = "everyone";
        public string WatchlistVisibility { get; set; } = "everyone";
        public string RatingsVisibility { get; set; } = "everyone";
        public bool HideEmail { get; set; } = true;
        public bool PersonalisedRecs { get; set; } = true;
    }
}
