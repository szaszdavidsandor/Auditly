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

        public async Task<UserResponse?> UpdateUserAsync(int id, UpdateUserRequest requestDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
                return null;

            bool isUpdated = false;

            if (!string.IsNullOrWhiteSpace(requestDto.FullName))
            {
                if (user.FullName != requestDto.FullName)
                {
                    user.FullName = requestDto.FullName;
                    isUpdated = true;
                }
            }

            if (!string.IsNullOrWhiteSpace(requestDto.Email))
            {
                if (user.Email != requestDto.Email)
                {
                    user.Email = requestDto.Email;
                    isUpdated = true;
                }
            }

            if (!string.IsNullOrWhiteSpace(requestDto.Password))
            {
                var passwordResult = _passwordHasher.Hash(requestDto.Password);
                if (!(user.PasswordHash == passwordResult.hash))
                {
                    user.PasswordHash = passwordResult.hash;
                    user.PasswordSalt = passwordResult.salt;
                    isUpdated = true;
                }
            }

            if (!isUpdated)
                return null;

            await _userRepository.UpdateAsync(user);

            UserResponse response = new UserResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };

            return response;
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