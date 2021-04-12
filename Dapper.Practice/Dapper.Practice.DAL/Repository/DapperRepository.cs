namespace Dapper.Practice.DAL.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading.Tasks;
    using Dapper;
    using Dapper.Practice.DAL.Utilities;

    public class DapperRepository<T>
    {
        private string connectionStr;
        private string tableName;
        private SQLQueryGenerator generator;

        public DapperRepository(string tableName)
        {
            this.tableName = tableName;

            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = ".\\SQLEXPRESS";
            builder.InitialCatalog = "MailDB";
            builder.IntegratedSecurity = true;
            this.connectionStr = builder.ToString();
            this.generator = SQLQueryGenerator.GetInstance();
        }

        public async void CreateAsync(T item)
        {
            var queryStr = this.generator.GenerateInsert<T>(this.tableName);

            var queryParams = new DynamicParameters();
            var props = typeof(T).GetProperties();
            this.FillParams(queryParams, props, item);

            using (var connection = new SqlConnection(this.connectionStr))
            {
                await connection.ExecuteAsync(queryStr, queryParams);
            }
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            var queryStr = this.generator.GenerateSelect(this.tableName);

            var result = new List<T>();
            using (var connection = new SqlConnection(this.connectionStr))
            {
                result.AddRange(await connection.QueryAsync<T>(queryStr));
            }

            return result;
        }

        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            var queryStr = this.generator.GenerateSelect(this.tableName, predicate);

            var result = new List<T>();
            using (var connection = new SqlConnection(this.connectionStr))
            {
                result.AddRange(await connection.QueryAsync<T>(queryStr));
            }

            return result;
        }

        public async Task<T> GetById(T item)
        {
            var queryStr = this.generator.GenerateSelectById<T>(this.tableName);

            var queryParams = new DynamicParameters();
            var props = typeof(T).GetProperties();

            if (props.Select(x => x.Name).Contains("Id"))
            {
                queryParams.Add("@Id", props.First(x => x.Name == "Id").GetValue(item));
            }
            else
            {
                return default;
            }

            T result;
            using (var connection = new SqlConnection(this.connectionStr))
            {
                result = await connection.QueryFirstOrDefaultAsync<T>(queryStr, queryParams);
            }

            return result;
        }

        public async void Update(T item)
        {
            var queryStr = this.generator.GenerateUpdate<T>(this.tableName);

            var queryParams = new DynamicParameters();
            var props = typeof(T).GetProperties();
            this.FillParams(queryParams, props, item, true);

            using (var connection = new SqlConnection(this.connectionStr))
            {
                await connection.ExecuteAsync(queryStr, queryParams);
            }
        }

        public async void Delete(T item)
        {
            var queryStr = this.generator.GenerateDelete<T>(this.tableName);

            var queryParams = new DynamicParameters();
            var props = typeof(T).GetProperties();

            if (props.Select(x => x.Name).Contains("Id"))
            {
                queryParams.Add("@Id", props.First(x => x.Name == "Id").GetValue(item));
            }
            else
            {
                this.FillParams(queryParams, props, item);
            }

            using (var connection = new SqlConnection(this.connectionStr))
            {
                await connection.ExecuteAsync(queryStr, queryParams);
            }
        }

        private void FillParams(DynamicParameters parameters, PropertyInfo[] properties, T item, bool includeId = false)
        {
            foreach (var prop in properties)
            {
                if (!includeId && prop.Name == "Id")
                {
                    continue;
                }

                parameters.Add("@" + prop.Name, prop.GetValue(item));
            }
        }
    }
}
