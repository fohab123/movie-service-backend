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
        Task<UserPrivacyDTO?> GetPrivacySettingsAsync(int userId);
        Task UpdatePrivacySettingsAsync(int userId, UserPrivacyDTO dto);
        Task<bool> UpdateEmailAsync(int userId, string email);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<bool> SoftDeleteUserAsync(int userId);
    }
}
