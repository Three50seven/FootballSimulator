using Common.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Common.Core
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Apply a predicate Where to a queryable if a given condition is met.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="condition"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IQueryable<T> WhereIf<T>(
            this IQueryable<T> query,
            bool condition,
            Expression<Func<T, bool>> predicate)
        {
            if (condition)
                return query.Where(predicate);
            else
                return query;
        }

        public static bool IsOrdered<T>(this IQueryable<T> queryable)
        {
            ArgumentNullException.ThrowIfNull(queryable);
            return queryable.Expression.Type == typeof(IOrderedQueryable<T>);
        }
    }
}
