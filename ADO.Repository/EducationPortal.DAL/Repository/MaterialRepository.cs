// <copyright file="MaterialRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EducationPortal.DAL.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq.Expressions;
    using EducationPortal.DAL.Entities;

    public class MaterialRepository : ADORepository<Material>
    {
        private Dictionary<Type, string> concreteMaterialUpdateQueries = new Dictionary<Type, string>()
        {
            { typeof(Article), "UPDATE Articles SET PublicationDate = @PublicationDate WHERE Id = @Id" },
            { typeof(Book),    "UPDATE Books SET AuthorNames = @AuthorNames, PageCount = @PageCount, Format = @Format, PublishingYear = @PublishingYear WHERE Id = @Id" },
            { typeof(Video),   "UPDATE Videos SET Duration = @Duration, Quality = @Quality WHERE Id = @Id" },
        };

        private Dictionary<Type, string> concreteMaterialInsertQueries = new Dictionary<Type, string>()
        {
            { typeof(Article), "INSERT INTO Articles(Id, PublicationDate) VALUES(@Id, @PublicationDate)" },
            { typeof(Book),    "INSERT INTO Books(Id, AuthorNames, PageCount, [Format], PublishingYear) VALUES (@Id, @AuthorNames, @PageCount, @Format, @PublishingYear)" },
            { typeof(Video),   "INSERT INTO Videos(Id, Duration, Quality) VALUES (@Id, @Duration, @Quality)" },
        };

        protected override string InsertQuery => "INSERT INTO Materials([Name], [Url]) OUTPUT inserted.id VALUES (@Name, @Url)";

        protected override string SelectQuery => @"SELECT  m.Id, a.Id, b.Id, v.Id, 
		                                                    m.Name, m.Url, 
                                                            a.PublicationDate, 
                                                            b.AuthorNames, b.PageCount, b.Format, b.PublishingYear, 
                                                            v.Duration, v.Quality 
                                                    FROM dbo.Materials m 
                                                            LEFT JOIN dbo.Books b ON m.Id = b.Id 
                                                            LEFT JOIN dbo.Articles a ON a.Id = m.Id 
                                                            LEFT JOIN dbo.Videos v ON v.Id = m.Id";

        protected override string UpdateQuery => "UPDATE Materials SET Name = @Name, Url = @Url WHERE Id = @Id";

        protected override string DeleteQuery => "DELETE FROM Materials WHERE Id = @Id";

        protected override string WhereIdClause => " WHERE m.Id = @Id";

        public override void Create(Material item)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                var command = new SqlCommand(this.InsertQuery, connection);
                command.Parameters.AddWithValue("@Name", item.Name);
                command.Parameters.AddWithValue("@Url", item.Url);

                item.Id = (int)command.ExecuteScalar();

                command = new SqlCommand(this.concreteMaterialInsertQueries[item.GetType()], connection);

                foreach (var prop in item.GetType().GetProperties())
                {
                    command.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(item));
                }

                command.ExecuteNonQuery();
            }
        }

        public override IEnumerable<Material> Find(Expression<Func<Material, bool>> predicate)
        {
            return this.SelectByQuery(this.SelectQuery + " WHERE " + this.queryHelper.GetSQLPredicate(predicate));
        }

        public override IEnumerable<Material> GetAll()
        {
            return this.SelectByQuery(this.SelectQuery);
        }

        public override Material GetById(long id)
        {
            Material material = null;
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                var command = new SqlCommand(this.SelectQuery + this.WhereIdClause, connection);
                command.Parameters.AddWithValue("@Id", id);

                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    var articleId = reader.GetValue(1);
                    var bookId = reader.GetValue(2);
                    var videoId = reader.GetValue(3);

                    if (!(articleId is DBNull))
                    {
                        material = new Article()
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(4),
                            Url = reader.GetString(5),
                            PublicationDate = reader.GetDateTime(6),
                        };
                    }
                    else if (!(bookId is DBNull))
                    {
                        material = new Book()
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(4),
                            Url = reader.GetString(5),
                            AuthorNames = reader.GetString(7),
                            PageCount = reader.GetInt32(8),
                            Format = reader.GetString(9),
                            PublishingYear = reader.GetInt16(10),
                        };
                    }
                    else if (!(videoId is DBNull))
                    {
                        material = new Video
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(4),
                            Url = reader.GetString(5),
                            Duration = reader.GetTimeSpan(11),
                            Quality = reader.GetString(12),
                        };
                    }
                }
            }

            return material;
        }

        public override void Update(Material item)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                var command = new SqlCommand(this.UpdateQuery, connection);
                command.Parameters.AddWithValue("@Name", item.Name);
                command.Parameters.AddWithValue("@Url", item.Url);
                command.Parameters.AddWithValue("@Id", item.Id);

                command.ExecuteNonQuery();

                command = new SqlCommand(this.concreteMaterialUpdateQueries[item.GetType()], connection);

                foreach (var prop in item.GetType().GetProperties())
                {
                    command.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(item));
                }

                command.ExecuteNonQuery();
            }
        }

        protected override IEnumerable<Material> SelectByQuery(string query)
        {
            var materials = new List<Material>();
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                var command = new SqlCommand(query, connection);

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

            return materials;
        }
    }
}
