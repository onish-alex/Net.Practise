// <copyright file="SkillRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EducationPortal.DAL.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq.Expressions;
    using EducationPortal.DAL.Entities;

    public class SkillRepository : ADORepository<Skill>
    {
        protected override string InsertQuery => "INSERT INTO Skills([Name]) VALUES (@Name)";

        protected override string SelectQuery => "SELECT Id, Name FROM Skills";

        protected override string UpdateQuery => "UPDATE Skills SET Name = @Name WHERE Id = @Id";

        protected override string DeleteQuery => "DELETE FROM Skills WHERE Id = @Id";

        public override void Create(Skill item)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                var command = new SqlCommand(this.InsertQuery, connection);
                command.Parameters.AddWithValue("@Name", item.Name);

                command.ExecuteNonQuery();
            }
        }

        public override IEnumerable<Skill> Find(Expression<Func<Skill, bool>> predicate)
        {
            return this.SelectByQuery(this.SelectQuery + " WHERE " + this.queryHelper.GetSQLPredicate(predicate));
        }

        public override IEnumerable<Skill> GetAll()
        {
            return this.SelectByQuery(this.SelectQuery);
        }

        public override Skill GetById(long id)
        {
            Skill skill = null;
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                var command = new SqlCommand(this.SelectQuery + this.WhereIdClause, connection);
                command.Parameters.AddWithValue("@Id", id);

                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    skill = new Skill
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                    };
                }
            }

            return skill;
        }

        public override void Update(Skill item)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                var command = new SqlCommand(this.UpdateQuery, connection);
                command.Parameters.AddWithValue("@Name", item.Name);
                command.Parameters.AddWithValue("@Id", item.Id);

                command.ExecuteNonQuery();
            }
        }

        protected override IEnumerable<Skill> SelectByQuery(string query)
        {
            var skills = new List<Skill>();
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                var command = new SqlCommand(query, connection);

                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var skill = new Skill
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                        };

                        skills.Add(skill);
                    }
                }
            }

            return skills;
        }
    }
}
