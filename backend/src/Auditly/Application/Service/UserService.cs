using Domain.Interface;
using Application.Interface;
using Domain.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTO;
using Application.DTO.Request;
using Application.DTO.Response;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return users.Select(user => new UserResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            });
        }

        public async Task<UserResponse?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null) 
                return null;

            return new UserResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<UserResponse> CreateUserAsync(CreateUserRequest requestDto)
        {
            var user = new User
            {
                FullName = requestDto.FullName,
                Email = requestDto.Email,
                PasswordHash = _passwordHasher.Hash(requestDto.Password).hash,
                PasswordSalt = _passwordHasher.Hash(requestDto.Password).salt,
            };

            var createdUser = await _userRepository.AddAsync(user);

            return new UserResponse
            {
                Id = createdUser.Id,
                FullName = createdUser.FullName,
                Email = createdUser.Email,
                Role = createdUser.Role,
                CreatedAt = createdUser.CreatedAt
            };
        }

        public async Task<bool> UpdateUserAsync(int id, UpdateUserRequest requestDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null) 
                return false;

            if(!string.IsNullOrEmpty(requestDto.FullName))
                user.FullName = requestDto.FullName;

            if (!string.IsNullOrEmpty(requestDto.Email))
                user.Email = requestDto.Email;

            if (!string.IsNullOrEmpty(requestDto.Password))
            {
                var passwordResult = _passwordHasher.Hash(requestDto.Password);
                user.PasswordHash = passwordResult.hash;
                user.PasswordSalt = passwordResult.salt;
            }

            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<bool> SoftDeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null) 
                return false;

            await _userRepository.SoftDeleteAsync(id);
            return true;
        }
    }
}