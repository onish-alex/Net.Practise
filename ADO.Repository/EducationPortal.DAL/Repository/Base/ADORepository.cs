// <copyright file="ADORepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EducationPortal.DAL.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Linq.Expressions;
    using EducationPortal.DAL.Entities;
    using EducationPortal.DAL.Utilities;

    public abstract class ADORepository<T> : IRepository<T>
        where T : Entity
    {
        protected string connectionString;
        protected QueryHelper queryHelper;

        public ADORepository()
        {
            this.connectionString = ConfigurationManager.ConnectionStrings["EducationPortal"].ConnectionString;

            this.queryHelper = QueryHelper.GetInstance();
        }

        protected abstract string InsertQuery { get; }

        protected abstract string SelectQuery { get; }

        protected abstract string UpdateQuery { get; }

        protected abstract string DeleteQuery { get; }

        protected virtual string WhereIdClause => " WHERE Id = @Id";

        public abstract void Create(T item);

        public virtual void Delete(int id)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                var command = new SqlCommand(this.DeleteQuery, connection);
                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
        }

        public abstract IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        public abstract IEnumerable<T> GetAll();

        public abstract T GetById(long id);

        public void Save()
        {
        }

        public abstract void Update(T item);

        protected abstract IEnumerable<T> SelectByQuery(string query);
    }
}
