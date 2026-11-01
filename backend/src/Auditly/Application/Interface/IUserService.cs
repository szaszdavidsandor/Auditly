using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTO;
using Application.DTO.Request;
using Application.DTO.Response;

namespace Application.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponse?>> GetAllUsersAsync();
        Task<UserResponse?> GetUserByIdAsync(int id);
        Task<UserResponse?> CreateUserAsync(CreateUserRequest requestDto);
        Task<bool> UpdateUserAsync(int id, UpdateUserRequest requestDto);
        Task<bool> SoftDeleteUserAsync(int id);
    }
}