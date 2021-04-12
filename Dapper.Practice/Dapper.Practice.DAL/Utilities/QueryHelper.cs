namespace Dapper.Practice.DAL.Utilities
{
    using System;
    using System.Linq.Expressions;
    using System.Text;

    public class QueryHelper
    {
        private static QueryHelper instance = new QueryHelper();

        private QueryHelper()
        {
        }

        public static QueryHelper GetInstance()
        {
            return instance;
        }

        public string GetSQLPredicate<T>(Expression<Func<T, bool>> predicate)
        {
            var builder = new StringBuilder(predicate.Body.ToString());

            var binary = (BinaryExpression)predicate.Body;

            var left = binary.Left;
            var right = binary.Right;

            builder.Replace(left.ToString(), ((MemberExpression)left).Member.Name);

            object value = null;

            if (right.NodeType == ExpressionType.MemberAccess)
            {
                var converted = Expression.Convert(right, typeof(object));
                var lambda = Expression.Lambda<Func<object>>(converted);
                value = lambda.Compile().Invoke();
            }
            else
            {
                value = ((ConstantExpression)right).Value;
            }

            if (value is string)
            {
                builder.Replace(right.ToString(), $"'{value}'");
            }
            else
            {
                builder.Replace(right.ToString(), value.ToString());
            }

            builder.Replace("==", "=");
            builder.Replace("AndAlso", "and");
            builder.Replace("OrElse", "or");
            builder.Replace("\"", "'");

            return builder.ToString();
        }
    }
}
