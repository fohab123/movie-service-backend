using movie_service_backend.DTO.UserDTOs;

namespace movie_service_backend.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserAdminDTO>> GetAllUsersAsync();
        Task<UserAdminDTO?> GetUserByIdAsync(int id);
        Task<UserAdminDTO> CreateUserAsync(UserCreateDTO dto);
        Task<UserAdminDTO> UpdateUserAsync(int id, UserCreateDTO dto);
        Task<bool> DeleteUserAsync(int id);
        Task<string?> LoginAsync(LoginDTO dto);
        Task<bool> VerifyEmailAsync(string token);
        Task<UserStatsDTO> GetUserStatsAsync(int userId);


    }
}
