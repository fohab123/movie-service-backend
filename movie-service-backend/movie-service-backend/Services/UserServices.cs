using AutoMapper;
using movie_service_backend.DTO.UserDTOs;
using movie_service_backend.Interfaces;
using movie_service_backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using movie_service_backend.Repo;

namespace movie_service_backend.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepo _repo;
        private readonly IMapper _mapper;
        private readonly PasswordService _passwordService;
        private readonly JwtService _jwtService;
        private readonly EmailService _emailService;

        public UserService(UserRepo repo, IMapper mapper, PasswordService passwordService, JwtService jwtService, EmailService emailService)
        {
            _repo = repo;
            _mapper = mapper;
            _passwordService = passwordService;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        public async Task<UserAdminDTO> CreateUserAsync(UserCreateDTO dto)
        {

            var users = _mapper.Map<User>(dto);
            users.Password = _passwordService.HashPassword(dto.Password);

            users.IsEmailVerified = false;
            users.EmailVerificationToken = Guid.NewGuid().ToString(); // generiši token

            await _repo.AddAsync(users);
            await _repo.SaveChangesAsync();

            await _emailService.SendVerificationEmailAsync(users.Email, users.EmailVerificationToken);

            return _mapper.Map<UserAdminDTO>(users);
        }

        public async Task<bool> VerifyEmailAsync(string token)
        {
            var user = (await _repo.GetAllAsync())
                .FirstOrDefault(u => u.EmailVerificationToken == token);

            if (user == null)
                return false;

            user.IsEmailVerified = true;
            user.EmailVerificationToken = null;

            _repo.Update(user);
            await _repo.SaveChangesAsync();

            return true;
        }



        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) return false;
            _repo.Delete(user);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<UserAdminDTO>> GetAllUsersAsync()
        {
            var users = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<UserAdminDTO>>(users);
        }

        public async Task<UserAdminDTO?> GetUserByIdAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            return user == null ? null : _mapper.Map<UserAdminDTO>(user);
        }

        public async Task<string?> LoginAsync(LoginDTO dto)
        {
            var user = (await _repo.GetAllAsync())
                .FirstOrDefault(u => string.Equals(u.Username, dto.Username, StringComparison.OrdinalIgnoreCase));

            if (user == null)
                return null;

            // 🚫 Ako email nije verifikovan — zabrani login
            if (!user.IsEmailVerified)
                throw new UnauthorizedAccessException("Email not verified. Please verify your email before logging in.");

            // 🔑 Provera lozinke
            if (!_passwordService.VerifyPassword(user.Password, dto.Password))
                return null;

            // ✅ Ako je sve ok — generiši JWT token
            return _jwtService.GenerateToken(user);
        }

        public async Task<UserAdminDTO> UpdateUserAsync(int id, UserCreateDTO dto)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) return null;
            user.Username = dto.Username;

            if (!string.IsNullOrEmpty(dto.Password))
            {
                user.Password = _passwordService.HashPassword(dto.Password);
            }
            _repo.Update(user);
            await _repo.SaveChangesAsync();

            return _mapper.Map<UserAdminDTO>(user);
        } 
        

    }
}
