// <copyright file="Video.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EducationPortal.DAL.Entities
{
    using System;

    public class Video : Material
    {
        public TimeSpan Duration { get; set; }

        public string Quality { get; set; }
    }
}
