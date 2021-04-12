// <copyright file="CourseRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EducationPortal.DAL.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq.Expressions;
    using EducationPortal.DAL.Entities;

    public class CourseRepository : ADORepository<Course>
    {
        private string selectCourseCreator = "SELECT Id, [Name], [Login], Email, Password FROM Users WHERE Id = @Id";

        private string selectCourseMaterialIds = @"SELECT  m.Id, a.Id, b.Id, v.Id, 
		                                                    m.Name, m.Url, 
                                                            a.PublicationDate, 
                                                            b.AuthorNames, b.PageCount, b.Format, b.PublishingYear, 
                                                            v.Duration, v.Quality 
                                                    FROM dbo.Materials m 
                                                         LEFT JOIN dbo.Books b ON m.Id = b.Id 
                                                         LEFT JOIN dbo.Articles a ON a.Id = m.Id 
                                                         LEFT JOIN dbo.Videos v ON v.Id = m.Id
                                                         JOIN CourseMaterial cm ON m.Id = cm.MaterialsId
                                                    WHERE cm.CoursesId = @Id";

        private string insertCourseMaterial = "INSERT INTO CourseMaterial VALUES (@CourseId, @MaterialId)";

        private string deleteCourseMaterial = "DELETE FROM CourseMaterial WHERE CoursesId = @CourseId and MaterialsId = @MaterialId";

        protected override string InsertQuery => "INSERT INTO Courses([Name], Description, CreatorId) VALUES (@Name, @Description, @CreatorId)";

        protected override string SelectQuery => "SELECT Id, Name, Description, CreatorId FROM Courses";

        protected override string UpdateQuery => "UPDATE Courses SET Name = @Name, Description = @Description, CreatorId = @CreatorId WHERE Id = @Id";

        protected override string DeleteQuery => "DELETE FROM Courses WHERE Id = @Id";

        public void AddCourseMaterial(long courseId, long materialId)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                var command = new SqlCommand(this.insertCourseMaterial, connection);
                command.Parameters.AddWithValue("@CourseId", courseId);
                command.Parameters.AddWithValue("@MaterialId", materialId);

                command.ExecuteNonQuery();
            }
        }

        public void DeleteCourseMaterial(long courseId, long materialId)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                var command = new SqlCommand(this.deleteCourseMaterial, connection);
                command.Parameters.AddWithValue("@CourseId", courseId);
                command.Parameters.AddWithValue("@MaterialId", materialId);

                command.ExecuteNonQuery();
            }
        }

        public override void Create(Course item)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                var command = new SqlCommand(this.InsertQuery, connection);
                command.Parameters.AddWithValue("@Name", item.Name);
                command.Parameters.AddWithValue("@Description", item.Description);
                command.Parameters.AddWithValue("@CreatorId", item.Creator.Id);
                command.Parameters.AddWithValue("@Id", item.Id);

                command.ExecuteNonQuery();
            }
        }

        public override IEnumerable<Course> Find(Expression<Func<Course, bool>> predicate)
        {
            return this.SelectByQuery(this.SelectQuery + " WHERE " + this.queryHelper.GetSQLPredicate(predicate));
        }

        public override IEnumerable<Course> GetAll()
        {
            return this.SelectByQuery(this.SelectQuery);
        }

        public override Course GetById(long id)
        {
            Course course = null;
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                var command = new SqlCommand(this.SelectQuery + this.WhereIdClause, connection);
                command.Parameters.AddWithValue("@Id", id);

                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    course = new Course
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                    };
                    var creatorId = reader.GetInt32(3);

                    this.FillMaterial(course);
                    this.FillCreator(course, creatorId);
                }
            }

            return course;
        }

        public override void Update(Course item)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                var command = new SqlCommand(this.UpdateQuery, connection);
                command.Parameters.AddWithValue("@Name", item.Name);
                command.Parameters.AddWithValue("@Description", item.Description);
                command.Parameters.AddWithValue("@CreatorId", item.Creator.Id);
                command.Parameters.AddWithValue("@Id", item.Id);

                command.ExecuteNonQuery();
            }
        }

        protected override IEnumerable<Course> SelectByQuery(string query)
        {
            var courses = new List<Course>();
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                var command = new SqlCommand(query, connection);

                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var course = new Course
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                        };
                        var creatorId = reader.GetInt32(3);

                        this.FillMaterial(course);
                        this.FillCreator(course, creatorId);

                        courses.Add(course);
                    }
                }
            }

            return courses;
        }

        private void FillMaterial(Course course)
        {
            var materials = new List<Material>();
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                var command = new SqlCommand(this.selectCourseMaterialIds, connection);
                command.Parameters.AddWithValue("@Id", course.Id);

                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var articleId = reader.GetValue(1);
                        var bookId = reader.GetValue(2);
                        var videoId = reader.GetValue(3);

                        if (!(articleId is DBNull))
                        {
                            materials.Add(new Article()
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(4),
                                Url = reader.GetString(5),
                                PublicationDate = reader.GetDateTime(6),
                            });
                        }
                        else if (!(bookId is DBNull))
                        {
                            materials.Add(new Book()
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(4),
                                Url = reader.GetString(5),
                                AuthorNames = reader.GetString(7),
                                PageCount = reader.GetInt32(8),
                                Format = reader.GetString(9),
                                PublishingYear = reader.GetInt16(10),
                            });
                        }
                        else if (!(videoId is DBNull))
                        {
                            materials.Add(new Video
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(4),
                                Url = reader.GetString(5),
                                Duration = reader.GetTimeSpan(11),
                                Quality = reader.GetString(12),
                            });
                        }
                    }
                }
            }

            course.Materials = materials;
        }

        private void FillCreator(Course course, int creatorId)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                var command = new SqlCommand(this.selectCourseCreator, connection);
                command.Parameters.AddWithValue("@Id", creatorId);

                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    var creator = new User()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Login = reader.GetString(2),
                        Email = reader.GetString(3),
                        Password = reader.GetString(4),
                    };
                    course.Creator = creator;
                }
            }
        }
    }
}
