namespace movie_service_backend.DTO.UserDTOs
{
    public class UserAdminDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
    }
}
