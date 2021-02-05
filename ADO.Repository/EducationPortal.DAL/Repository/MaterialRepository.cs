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

        protected override string SelectQuery => @"SELECT m.Id, m.Name, m.Url, 
                                                          a.PublicationDate, 
                                                          b.AuthorNames, b.PageCount, b.Format, b.PublishingYear, 
                                                          v.Duration, v.Quality 
                                                    FROM Materials m 
                                                         LEFT JOIN Books b ON m.Id = b.Id 
                                                         LEFT JOIN Articles a ON a.Id = m.Id 
                                                         LEFT JOIN Videos v ON v.Id = m.Id";

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
                command.Parameters.AddWithValue("@Id", item.Id);

                item.Id = (int)command.ExecuteScalar();

                command = new SqlCommand(this.concreteMaterialInsertQueries[item.GetType()], connection);
                command.Parameters.AddWithValue("@Id", item.Id);

                switch (item)
                {
                    case Book book:
                        command.Parameters.AddWithValue("@AuthorNames", book.AuthorNames);
                        command.Parameters.AddWithValue("@Format", book.Format);
                        command.Parameters.AddWithValue("@PageCount", book.PageCount);
                        command.Parameters.AddWithValue("@PublishingYear", book.PublishingYear);
                        break;

                    case Article article:
                        command.Parameters.AddWithValue("@PublicationDate", article.PublicationDate);
                        break;

                    case Video video:
                        command.Parameters.AddWithValue("@Duration", video.Duration);
                        command.Parameters.AddWithValue("@Quality", video.Quality);
                        break;
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
                    try
                    {
                        TimeSpan duration = reader.GetTimeSpan(8);

                        material = new Video
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Url = reader.GetString(2),
                            Duration = duration,
                            Quality = reader.GetString(9),
                        };
                    }
                    catch
                    {
                    }

                    try
                    {
                        DateTime publicationDate = reader.GetDateTime(3);

                        material = new Article()
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Url = reader.GetString(2),
                            PublicationDate = publicationDate,
                        };
                    }
                    catch
                    {
                    }

                    try
                    {
                        var authors = reader.GetString(4);

                        material = new Book()
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Url = reader.GetString(2),
                            AuthorNames = authors,
                            PageCount = reader.GetInt32(5),
                            Format = reader.GetString(6),
                            PublishingYear = reader.GetInt16(7),
                        };
                    }
                    catch
                    {
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
                command.Parameters.AddWithValue("@Id", item.Id);

                switch (item)
                {
                    case Book book:
                        command.Parameters.AddWithValue("@AuthorNames", book.AuthorNames);
                        command.Parameters.AddWithValue("@Format", book.Format);
                        command.Parameters.AddWithValue("@PageCount", book.PageCount);
                        command.Parameters.AddWithValue("@PublishingYear", book.PublishingYear);
                        break;

                    case Article article:
                        command.Parameters.AddWithValue("@PublicationDate", article.PublicationDate);
                        break;

                    case Video video:
                        command.Parameters.AddWithValue("@Duration", video.Duration);
                        command.Parameters.AddWithValue("@Quality", video.Quality);
                        break;
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
                        try
                        {
                            TimeSpan duration = reader.GetTimeSpan(8);

                            var video = new Video
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Url = reader.GetString(2),
                                Duration = duration,
                                Quality = reader.GetString(9),
                            };

                            materials.Add(video);
                            continue;
                        }
                        catch
                        {
                        }

                        try
                        {
                            DateTime publicationDate = reader.GetDateTime(3);

                            var article = new Article()
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Url = reader.GetString(2),
                                PublicationDate = publicationDate,
                            };
                            materials.Add(article);
                            continue;
                        }
                        catch
                        {
                        }

                        try
                        {
                            var authors = reader.GetString(4);

                            var book = new Book()
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Url = reader.GetString(2),
                                AuthorNames = authors,
                                PageCount = reader.GetInt32(5),
                                Format = reader.GetString(6),
                                PublishingYear = reader.GetInt16(7),
                            };
                            materials.Add(book);
                            continue;
                        }
                        catch
                        {
                        }
                    }
                }
            }

            return materials;
        }
    }
}
