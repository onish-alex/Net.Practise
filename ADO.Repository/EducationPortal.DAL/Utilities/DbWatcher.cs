// <copyright file="DbWatcher.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

// Obsolete: type from file repository version
namespace EducationPortal.DAL.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class DbWatcher
    {
        private static readonly DbWatcher Instance = new DbWatcher();
        private Dictionary<TableNames, FileSystemWatcher> watchers;

        private DbWatcher()
        {
            this.watchers = new Dictionary<TableNames, FileSystemWatcher>();
            var tableNames = Enum.GetNames(typeof(TableNames));
            foreach (var name in tableNames)
            {
                var tableEnumValue = (TableNames)Enum.Parse(typeof(TableNames), name);
                var tablePath = DbConfig.TablePaths[tableEnumValue];
                this.watchers.Add(tableEnumValue, new FileSystemWatcher(DbConfig.DbPathPrefix + tablePath));
                this.watchers[tableEnumValue].EnableRaisingEvents = true;
            }
        }

        public FileSystemWatcher this[TableNames tableName]
        {
            get
            {
                return this.watchers[tableName];
            }
        }

        public static DbWatcher GetInstance()
        {
            return Instance;
        }
    }
}
