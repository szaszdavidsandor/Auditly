using Application.DTO;
using Application.DTO.Request;
using Application.DTO.Response;
using Domain.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponse?>> GetAllUsersAsync();
        Task<UserResponse?> GetUserByIdAsync(int id);
        Task<UserResponse?> CreateUserAsync(CreateUserRequest requestDto);
        Task<UserResponse?> UpdateUserAsync(int id, UpdateUserRequest requestDto);
        Task<bool> SoftDeleteUserAsync(int id);
    }
}