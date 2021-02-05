// <copyright file="JsonFileHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EducationPortal.DAL.Utilities
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using EducationPortal.DAL.Entities;
    using Newtonsoft.Json;

    public class JsonFileHelper
    {
        private static readonly string Extension = ".json";
        private static readonly JsonFileHelper Instance = new JsonFileHelper();

        private JsonSerializer serializer;
        private Mutex mutex;

        private JsonFileHelper()
        {
            this.serializer = new JsonSerializer
            {
                DateFormatString = "yyyy-MM-dd",
            };
            this.mutex = new Mutex(false, @"Global/EducationPortal");
        }

        public static JsonFileHelper GetInstance()
        {
            return Instance;
        }

        public void SaveTable(string tablePath, IDictionary<Entity, EntityState> content)
        {
            this.mutex.WaitOne();
            var hasGotNextId = int.TryParse(File.ReadAllText(tablePath + DbConfig.DbIdsFileName), out int nextId);
            this.mutex.ReleaseMutex();

            if (!hasGotNextId)
            {
                nextId = 1;
            }

            foreach (var item in content)
            {
                if (item.Value == EntityState.Created)
                {
                    item.Key.Id = nextId++;
                }

                var fileName = tablePath + item.Key.Id + Extension;

                if (item.Value == EntityState.Deleted)
                {
                    File.Delete(fileName);
                    continue;
                }

                var fileInfo = new FileInfo(fileName);
                this.mutex.WaitOne();

                using (StreamWriter sw = fileInfo.CreateText())
                {
                    using (JsonWriter jw = new JsonTextWriter(sw))
                    {
                        this.serializer.Serialize(jw, item.Key);
                    }
                }

                this.mutex.ReleaseMutex();
            }

            this.mutex.WaitOne();
            File.WriteAllText(tablePath + DbConfig.DbIdsFileName, nextId.ToString());
            this.mutex.ReleaseMutex();
        }

        public IEnumerable<T> ReadTable<T>(string tablePath)
            where T : Entity
        {
            var entityPaths = Directory.EnumerateFiles(tablePath).Where(file => Path.GetRelativePath(tablePath, file) != DbConfig.DbIdsFileName);
            var tableContent = new List<T>();
            this.mutex.WaitOne();
            foreach (var entity in entityPaths)
            {
                using (StreamReader sr = new StreamReader(entity))
                {
                    using (JsonReader jr = new JsonTextReader(sr))
                    {
                        tableContent.Add(this.serializer.Deserialize<T>(jr));
                    }
                }
            }

            this.mutex.ReleaseMutex();

            return tableContent;
        }

        public T ReadEntity<T>(string tablePath, long id)
            where T : Entity
        {
            var fileName = tablePath + id + Extension;
            T entity;

            this.mutex.WaitOne();

            using (StreamReader sr = new StreamReader(fileName))
            {
                using (JsonReader jr = new JsonTextReader(sr))
                {
                    entity = this.serializer.Deserialize<T>(jr);
                }
            }

            this.mutex.ReleaseMutex();

            return entity;
        }
    }
}
