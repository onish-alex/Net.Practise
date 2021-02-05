// <copyright file="QueryHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EducationPortal.DAL.Utilities
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

            for (int i = 0; i < predicate.Parameters.Count; i++)
            {
                builder.Replace(predicate.Parameters[i].Name + ".", string.Empty);
            }

            builder.Replace("==", "=");
            builder.Replace("AndAlso", "and");
            builder.Replace("OrElse", "or");
            builder.Replace("\"", "'");

            return builder.ToString();
        }
    }
}
