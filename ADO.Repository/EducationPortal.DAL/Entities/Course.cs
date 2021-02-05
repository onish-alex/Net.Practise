// <copyright file="Course.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EducationPortal.DAL.Entities
{
    using System.Collections.Generic;

    public class Course : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public User Creator { get; set; }

        public IEnumerable<Material> Materials { get; set; }

        public IEnumerable<Skill> Skills { get; set; }
    }
}
