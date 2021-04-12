// <copyright file="DbConfig.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

// Obsolete: type from file repository version
namespace EducationPortal.DAL.Utilities
{
    using System.Collections.Generic;

    public static class DbConfig
    {
        public static readonly IDictionary<TableNames, string> TablePaths = new Dictionary<TableNames, string>()
        {
            { TableNames.User, "users/" },
            { TableNames.Account, "accounts/" },
            { TableNames.Course, "courses/" },
            { TableNames.Material, "materials/" },
            { TableNames.Skill, "skills/" },
        };

        public static string DbPathPrefix = "../../../../db/";
        public static string DbIdsFileName = "ids.txt";
    }
}
