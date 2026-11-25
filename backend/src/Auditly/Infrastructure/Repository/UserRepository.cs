using Application.Exceptions;
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
            if (id <= 0)
                GlobalExceptionHandler.ThrowInvalidArgument();

            return await _dbSet.FindAsync(id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                GlobalExceptionHandler.ThrowInvalidArgument();

            var user = await _dbSet.FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }

        public IQueryable<User?> GetQueryable()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<User?> AddAsync(User user)
        {
            if (user is null)
                GlobalExceptionHandler.ThrowInvalidArgument();

            await _dbSet.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> UpdateAsync(User user)
        {
            if (user is null)
                GlobalExceptionHandler.ThrowInvalidArgument();

            _dbSet.Update(user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                GlobalExceptionHandler.ThrowNotFound();
            }

            return user;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            if (id <= 0)
                GlobalExceptionHandler.ThrowInvalidArgument();

            var user = await _dbSet.FindAsync(id);
            if (user == null)
                GlobalExceptionHandler.ThrowNotFound();

            _dbSet.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}