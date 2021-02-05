// <copyright file="DbTable.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

// Obsolete: type from file repository version
namespace EducationPortal.DAL.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EducationPortal.DAL.Entities;

    public class DbTable : IDbTable
    {
        private Dictionary<Entity, EntityState> content;

        public DbTable()
        {
            this.content = new Dictionary<Entity, EntityState>();
        }

        public DbTable(IEnumerable<Entity> starterSet)
            : this()
        {
            if (starterSet != null)
            {
                foreach (var item in starterSet)
                {
                    this.content.Add(item, EntityState.Synchronized);
                }
            }
        }

        public IEnumerable<Entity> Content
        {
            get
            {
                return this.content
                            .Keys
                            .Where(a => this.content[a] != EntityState.Deleted)
                            .AsEnumerable();
            }
        }

        public IEnumerable<KeyValuePair<Entity, EntityState>> ContentWithState { get => this.content.AsEnumerable(); }

        public Entity GetById(long id)
        {
            return this.content.Keys.SingleOrDefault(a => a.Id == id);
        }

        public EntityState GetEntityState(Entity item)
        {
            var withExistedId = this.GetById(item.Id);

            if (withExistedId != null)
            {
                return this.content[withExistedId];
            }

            return EntityState.NotFound;
        }

        public void SetEntityState(Entity item, EntityState state)
        {
            var withExistedId = this.GetById(item.Id);

            if (withExistedId != null)
            {
                this.content[withExistedId] = state;
            }
        }

        public void Add(Entity item)
        {
            this.content.Add(item, EntityState.Created);
        }

        public void Update(Entity item)
        {
            var withExistedId = this.GetById(item.Id);

            if (withExistedId != null)
            {
                this.content.Remove(withExistedId);
                this.content.Add(item, EntityState.Updated);
            }
        }

        public void Remove(Entity item)
        {
            var withExistedId = this.GetById(item.Id);

            if (withExistedId != null)
            {
                this.content[withExistedId] = EntityState.Deleted;
            }
        }

        public void RemoveFromTable(Entity item)
        {
            var withExistedId = this.GetById(item.Id);

            if (withExistedId != null)
            {
                this.content.Remove(withExistedId);
            }
        }

        public IEnumerable<Entity> Find(Func<Entity, bool> predicate) => this.content.Keys.Where(predicate);
    }
}
