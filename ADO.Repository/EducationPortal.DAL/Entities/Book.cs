// <copyright file="Book.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EducationPortal.DAL.Entities
{
    public class Book : Material
    {
        public string AuthorNames { get; set; }

        public int PageCount { get; set; }

        public string Format { get; set; }

        public int PublishingYear { get; set; }
    }
}
