using Domain.Interface;
using Domain.Model;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ConnectionDbContext _context;
        private readonly DbSet<User> _dbSet;

        public UserRepository(ConnectionDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<User>();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<User?> GetByUsernameOrEmailAsync(string fullName, string email)
        {
            if (string.IsNullOrEmpty(fullName) && string.IsNullOrEmpty(email))
                return null;

            var user = await _dbSet.FirstOrDefaultAsync(e =>
                e.FullName == fullName || e.Email == email);

            return user;
        }

        public IQueryable<User> GetQueryable()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<User> AddAsync(User entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<User> UpdateAsync(User entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<User> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                throw new InvalidOperationException("Entity not found");

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}