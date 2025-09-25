namespace movie_service_backend.DTO.UserDTOs
{
    public class LoginDTO
    {
        public string Username { get; set; }  // ili Email, zavisi kako se loguje
        public string Password { get; set; }
    }
}
