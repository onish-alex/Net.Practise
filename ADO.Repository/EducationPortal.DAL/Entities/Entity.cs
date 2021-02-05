// <copyright file="Entity.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EducationPortal.DAL.Entities
{
    using System;

    public abstract class Entity : ICloneable
    {
        public int Id { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
