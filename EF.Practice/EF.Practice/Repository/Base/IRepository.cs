namespace EF.Practice.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public interface IRepository<T>
        where T : class
    {
        public void Create(T item);

        public void Update(T item);

        public void Delete(T item);

        public T GetById(int id);

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        public IEnumerable<T> GetAll();

        public void Save();
    }
}
