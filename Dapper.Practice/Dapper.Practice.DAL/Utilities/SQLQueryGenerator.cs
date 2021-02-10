namespace Dapper.Practice.DAL.Utilities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;

    public class SQLQueryGenerator
    {
        private static SQLQueryGenerator instance = new SQLQueryGenerator();
        private StringBuilder queryBuilder;
        private QueryHelper helper;

        private SQLQueryGenerator()
        {
            this.queryBuilder = new StringBuilder();
            this.helper = QueryHelper.GetInstance();
        }

        public static SQLQueryGenerator GetInstance()
        {
            return instance;
        }

        public string GenerateInsert<T>(string tableName)
        {
            this.queryBuilder.Clear();
            this.queryBuilder.Append($"INSERT INTO {tableName} VALUES (");

            var props = typeof(T).GetProperties();

            foreach (var prop in props)
            {
                if (prop.Name == "Id")
                {
                    continue;
                }

                this.queryBuilder.Append($"@{prop.Name}, ");
            }

            this.queryBuilder.Remove(this.queryBuilder.Length - 2, 1);

            this.queryBuilder.Append(')');

            return this.queryBuilder.ToString();
        }

        public string GenerateUpdate<T>(string tableName)
        {
            this.queryBuilder.Clear();
            this.queryBuilder.Append($"UPDATE {tableName} SET ");

            var props = typeof(T).GetProperties();

            this.FillParams<T>(props, ",");

            this.queryBuilder.Append("WHERE ");

            if (props.Select(x => x.Name).Contains("Id"))
            {
                this.queryBuilder.Append("Id = @Id");
            }
            else
            {
                this.FillParams<T>(props.Where(x => x.Name.EndsWith("Id")).ToArray(), "and");
            }

            return this.queryBuilder.ToString();
        }

        public string GenerateDelete<T>(string tableName)
        {
            this.queryBuilder.Clear();
            this.queryBuilder.Append($"DELETE FROM {tableName} WHERE ");

            var props = typeof(T).GetProperties();

            if (props.Select(x => x.Name).Contains("Id"))
            {
                this.queryBuilder.Append("Id = @Id");
            }
            else
            {
                this.FillParams<T>(props, "and");
            }

            return this.queryBuilder.ToString();
        }

        public string GenerateSelect(string tableName)
        {
            this.queryBuilder.Clear();
            this.queryBuilder.Append($"SELECT * FROM {tableName} ");
            return this.queryBuilder.ToString();
        }

        public string GenerateSelect<T>(string tableName, Expression<Func<T, bool>> predicate)
        {
            this.GenerateSelect(tableName);
            this.queryBuilder.Append($"WHERE {this.helper.GetSQLPredicate(predicate)}");
            return this.queryBuilder.ToString();
        }

        public string GenerateSelectById<T>(string tableName)
        {
            this.queryBuilder.Clear();
            this.queryBuilder.Append($"SELECT * FROM {tableName} ");
            this.queryBuilder.Append("WHERE ");

            var props = typeof(T).GetProperties();

            if (props.Select(x => x.Name).Contains("Id"))
            {
                this.queryBuilder.Append("Id = @Id");
            }
            else
            {
                this.FillParams<T>(props, "and");
            }

            return this.queryBuilder.ToString();
        }

        private void FillParams<T>(PropertyInfo[] properties, string delimiter)
        {
            foreach (var prop in properties)
            {
                if (prop.Name == "Id")
                {
                    continue;
                }

                var columnAttribute = (ColumnAttribute)Attribute.GetCustomAttribute(prop, typeof(ColumnAttribute));

                if (columnAttribute == null)
                {
                    this.queryBuilder.Append($"{prop.Name} = ");
                }
                else
                {
                    this.queryBuilder.Append($"{columnAttribute.Name} = ");
                }

                this.queryBuilder.Append($"@{prop.Name} {delimiter} ");
            }

            this.queryBuilder.Remove(this.queryBuilder.Length - 1 - delimiter.Length, delimiter.Length + 1);
        }
    }
}
