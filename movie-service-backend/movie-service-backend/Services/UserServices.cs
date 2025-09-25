using AutoMapper;
using movie_service_backend.DTO.UserDTOs;
using movie_service_backend.Interfaces;
using movie_service_backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace movie_service_backend.Services
{
    public class UserService : IUserService
    {
        private readonly IRepo<User> _repo;
        private readonly IMapper _mapper;
        private readonly PasswordService _passwordService;
        private readonly JwtService _jwtService;

        public UserService(IRepo<User> repo, IMapper mapper, PasswordService passwordService, JwtService jwtService)
        {
            _repo = repo;
            _mapper = mapper;
            _passwordService = passwordService;
            _jwtService = jwtService;
        }

        public async Task<UserAdminDTO> CreateUserAsync(UserCreateDTO dto)
        {

            var users = _mapper.Map<User>(dto);
            users.Password = _passwordService.HashPassword(dto.Password);
            await _repo.AddAsync(users);
            await _repo.SaveChangesAsync();
            return _mapper.Map<UserAdminDTO>(users);
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

            if (user == null) return null;

            if (!_passwordService.VerifyPassword(user.Password, dto.Password))
                return null;

            // generiše JWT token
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
