using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace App.DataAccess.Repository
{
    public interface IRepository<T>
    {
        Task CreateAsync(T entity);

        Task UpdateAsync(T entity);

        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression);

        Task<T> GetByIdWithIncludesAsync(int id, IEnumerable<Expression<Func<T, dynamic>>> includes = null, bool isNoTracking = true);

        Task<bool> ExistAsync(Expression<Func<T, bool>> predicat);
    }
}