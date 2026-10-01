using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T?> GetByIdAsync(int id);

        IQueryable<T> GetQueryable();

        Task AddAsync(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}
