// <copyright file="IDbTable.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

// Obsolete: type from file repository version
namespace EducationPortal.DAL.Utilities
{
    using System;
    using System.Collections.Generic;
    using EducationPortal.DAL.Entities;

    public interface IDbTable
    {
        public IEnumerable<Entity> Content { get; }

        public void Add(Entity item);

        public void Update(Entity item);

        public void Remove(Entity item);

        public IEnumerable<Entity> Find(Func<Entity, bool> predicate);
    }
}
