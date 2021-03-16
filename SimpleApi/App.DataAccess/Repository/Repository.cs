using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using App.Core.Entity;
using Microsoft.EntityFrameworkCore;
using AppContext = App.DataAccess.Context.AppContext;

namespace App.DataAccess.Repository
{
    internal class Repository<T> : IRepository<T>
        where T : BaseEntity
    {
        private readonly AppContext dbContext;

        public Repository(AppContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateAsync(T entity)
        {
            await this.dbContext.Set<T>().AddAsync(entity);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            this.dbContext.Entry(entity).State = EntityState.Modified;
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression)
        {
            return await this.dbContext.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<T> GetByIdWithIncludesAsync(int id, IEnumerable<Expression<Func<T, dynamic>>> includes = null, bool isNoTracking = true)
        {
            var query = isNoTracking ? this.dbContext.Set<T>().AsNoTracking() : this.dbContext.Set<T>().AsQueryable();

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return await query.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> ExistAsync(Expression<Func<T, bool>> predicat)
        {
            return await this.dbContext.Set<T>().AnyAsync(predicat);
        }
    }
}