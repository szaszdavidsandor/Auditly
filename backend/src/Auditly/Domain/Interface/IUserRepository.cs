using Domain.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();

        Task<User?> GetByIdAsync(int id);

        Task<User?> GetByUsernameOrEmailAsync(string username, string email);

        IQueryable<User> GetQueryable();

        Task<User> AddAsync(User entity);

        Task<User> UpdateAsync(User entity);

        Task<User> DeleteAsync(int id);
    }
}