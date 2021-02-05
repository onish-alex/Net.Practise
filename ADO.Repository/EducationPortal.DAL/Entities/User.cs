// <copyright file="User.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EducationPortal.DAL.Entities
{
    using System.Collections.Generic;

    public class User : Entity
    {
        public string Name { get; set; }

        public string Login { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public IEnumerable<Course> JoinedCourses { get; set; }

        public IEnumerable<Course> CompletedCourses { get; set; }

        public IEnumerable<Material> LearnedMaterials { get; set; }

        public IDictionary<Skill, int> Skills { get; set; }
    }
}
