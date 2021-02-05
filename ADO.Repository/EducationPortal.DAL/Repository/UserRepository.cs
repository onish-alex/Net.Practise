// <copyright file="UserRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EducationPortal.DAL.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq.Expressions;
    using EducationPortal.DAL.Entities;

    public class UserRepository : ADORepository<User>
    {
        private string selectSkillsQuery = "SELECT s.Id, s.Name, us.Level FROM Skills s JOIN UserSkills_MtM us ON s.Id = us.SkillId WHERE us.UserId = @Id";

        private string insertUserSkillQuery = "INSERT INTO UserSkills_MtM(UserId, SkillId, Level) VALUES (@UserId, @SkillId, @Level)";

        private string updateUserSkillQuery = "UPDATE UserSkills_MtM SET Level = @Level WHERE UserId = @UserId and SkillId = @SkillId";

        protected override string InsertQuery => "INSERT INTO Users([Name], [Login], Email, Password) VALUES(@Name, @Login, @Email, @Password)";

        protected override string SelectQuery => "SELECT Id, [Name], [Login], Email, Password FROM Users";

        protected override string UpdateQuery => "UPDATE Users SET Name = @Name, Login = @Login, Email = @Email, Password = @Password WHERE Id = @Id";

        protected override string DeleteQuery => "DELETE FROM Users WHERE Id = @Id";

        public override void Create(User item)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                var command = new SqlCommand(this.InsertQuery, connection);
                command.Parameters.AddWithValue("@Name", item.Name);
                command.Parameters.AddWithValue("@Login", item.Login);
                command.Parameters.AddWithValue("@Email", item.Email);
                command.Parameters.AddWithValue("@Password", item.Password);
                command.Parameters.AddWithValue("@Id", item.Id);

                command.ExecuteNonQuery();
            }
        }

        public override IEnumerable<User> Find(Expression<Func<User, bool>> predicate)
        {
            return this.SelectByQuery(this.SelectQuery + " WHERE " + this.queryHelper.GetSQLPredicate(predicate));
        }

        public override IEnumerable<User> GetAll()
        {
            return this.SelectByQuery(this.SelectQuery);
        }

        public override User GetById(long id)
        {
            User user = null;
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                var command = new SqlCommand(this.SelectQuery + this.WhereIdClause, connection);
                command.Parameters.AddWithValue("@Id", id);

                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    user = new User
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Login = reader.GetString(2),
                        Email = reader.GetString(3),
                        Password = reader.GetString(4),
                    };

                    this.FillUserSkills(user);
                }
            }

            return user;
        }

        public override void Update(User item)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                var command = new SqlCommand(this.UpdateQuery, connection);
                command.Parameters.AddWithValue("@Name", item.Name);
                command.Parameters.AddWithValue("@Login", item.Login);
                command.Parameters.AddWithValue("@Email", item.Email);
                command.Parameters.AddWithValue("@Password", item.Password);
                command.Parameters.AddWithValue("@Id", item.Id);

                command.ExecuteNonQuery();
            }
        }

        public void AddSkill(long userId, long skillId, int level)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                var command = new SqlCommand(this.insertUserSkillQuery, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@skillId", skillId);
                command.Parameters.AddWithValue("@Level", level);

                command.ExecuteNonQuery();
            }
        }

        public void UpdateSkill(long userId, long skillId, int level)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                var command = new SqlCommand(this.updateUserSkillQuery, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@skillId", skillId);
                command.Parameters.AddWithValue("@Level", level);

                command.ExecuteNonQuery();
            }
        }

        protected override IEnumerable<User> SelectByQuery(string query)
        {
            var users = new List<User>();
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                var command = new SqlCommand(query, connection);

                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var user = new User
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Login = reader.GetString(2),
                            Email = reader.GetString(3),
                            Password = reader.GetString(4),
                        };

                        this.FillUserSkills(user);

                        users.Add(user);
                    }
                }
            }

            return users;
        }

        private void FillUserSkills(User user)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                var command = new SqlCommand(this.selectSkillsQuery, connection);
                command.Parameters.AddWithValue("@Id", user.Id);

                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    var skills = new Dictionary<Skill, int>();
                    while (reader.Read())
                    {
                        var skill = new Skill
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                        };
                        skills.Add(skill, reader.GetInt32(2));
                    }

                    user.Skills = skills;
                }
            }
        }
    }
}
