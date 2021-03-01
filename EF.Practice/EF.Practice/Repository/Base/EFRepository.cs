namespace EF.Practice.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Microsoft.EntityFrameworkCore;

    public abstract class EFRepository<T> : IRepository<T>
        where T : class
    {
        protected DbContext db;

        public EFRepository(DbContext db)
        {
            this.db = db;
        }

        protected abstract IQueryable<T> SelectQuery { get; }

        public void Create(T item)
        {
            this.db.Set<T>().Add(item);
        }

        public void Delete(T item)
        {
            this.db.Set<T>().Remove(item);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return this.SelectQuery
                .Where(predicate);
        }

        public IEnumerable<T> GetAll()
        {
            return this.SelectQuery;
        }

        public abstract T GetById(int id);

        public void Save()
        {
            this.db.SaveChanges();
        }

        public void Update(T item)
        {
            this.db.Set<T>().Update(item);
        }
    }
}
