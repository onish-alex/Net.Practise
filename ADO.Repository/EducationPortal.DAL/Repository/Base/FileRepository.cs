// <copyright file="FileRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EducationPortal.DAL.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using EducationPortal.DAL.DbContexts;
    using EducationPortal.DAL.Entities;

    public class FileRepository<T> : IRepository<T>
        where T : Entity
    {
        private FileDbContext db;

        public FileRepository(FileDbContext context)
        {
            this.db = context;
        }

        public void Create(T item) => this.db.GetTable<T>().Add(item);

        public void Delete(int id)
        {
            var entities = this.db.GetTable<T>();
            var entityToRemove = entities.Find(a => a.Id == id)
                                         .SingleOrDefault();

            if (entityToRemove != null)
            {
                entities.Remove(entityToRemove);
            }
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate) => this.GetAll().Where(predicate.Compile());

        public IEnumerable<T> GetAll() => this.db.GetTable<T>()
                                              .Content
                                              .Select(a => (T)a.Clone());

        public T GetById(long id)
        {
            var entities = this.db.GetTable<T>();
            var entityWithId = entities
                                .Find(a => a.Id == id)
                                .SingleOrDefault();

            if (entityWithId != null)
            {
                return entityWithId.Clone() as T;
            }

            return null;
        }

        public void Update(T item)
        {
            this.db.GetTable<T>()
                   .Update(item);
        }

        public void Save()
        {
            this.db.Save<T>();
        }
    }
}
