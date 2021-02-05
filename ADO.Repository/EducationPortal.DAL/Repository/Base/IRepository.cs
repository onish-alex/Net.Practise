// <copyright file="IRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EducationPortal.DAL.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using EducationPortal.DAL.Entities;

    public interface IRepository<T>
        where T : Entity
    {
        IEnumerable<T> GetAll();

        T GetById(long id);

        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        void Create(T item);

        void Update(T item);

        void Delete(int id);

        void Save();
    }
}
