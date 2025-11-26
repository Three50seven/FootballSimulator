using Common.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Common.Core
{
    public static class SortQueryExtensions
    {
        private static object InvokeSortingOperation<T>(this IQueryable<T> source, SortCriteria sort, bool continuation)
        {
            string[] props = sort.SortBy.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "p");
            Expression expr = arg;

            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty(prop);
                if (pi == null)
                    throw new InvalidOperationException($"Property not found on OrderBy attempt. IQueryable Item Type: {typeof(T).FullName} | Property Type: {type.FullName} | Property Name: '{prop}' | Full Property Reference: '{sort.SortBy}'.");

                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }

            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            return typeof(Queryable).GetMethods().Single(
                    method => method.Name == sort.GetQuerySortOperation(continuation)
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { source, lambda });
        }

        private static object InvokeSortingOperation<T>(this IEnumerable<T> source, SortCriteria sort, bool continuation)
        {
            string[] props = sort.SortBy.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "p");
            Expression expr = arg;

            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty(prop);
                if (pi == null)
                    throw new InvalidOperationException($"Property not found on OrderBy attempt. IEnumerable Item Type: {typeof(T).FullName} | Property Type: {type.FullName} | Property Name: '{prop}' | Full Property Reference: '{sort.SortBy}'.");

                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }

            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            return typeof(Enumerable).GetMethods().Single(
                    method => method.Name == sort.GetQuerySortOperation(continuation)
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { source, lambda.Compile() }); // .Compile() call required for IEnumerable (not for IQueryable)
        }

        /// <summary>
        /// Order query by matching property string name <paramref name="orderBy"/>
        /// against the custom type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string orderBy)
        {
            return source.OrderBy(new SortCriteria(orderBy));
        }

        /// <summary>
        /// Order query by matching property string name <paramref name="orderBy"/>
        /// against the custom type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderBy"></param>
        /// <param name="direction">Sort direction.</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string orderBy, SortDirectionOption direction)
        {
            return source.OrderBy(new SortCriteria(orderBy, direction));
        }

        /// <summary>
        /// Order query based on provided sort info. 
        /// Uses reflection and expression trees to dynamically generate the OrderBy statement.
        /// Reference - http://stackoverflow.com/a/233505
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, SortCriteria sort)
        {
            if (sort == null)
                return source.OrderBy(x => x);

            return (IOrderedQueryable<T>)InvokeSortingOperation(source, sort, continuation: false);
        }

        /// <summary>
        /// Continuation order query (then by) by matching property string name <paramref name="orderBy"/>
        /// against the custom type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string orderBy)
        {
            return source.ThenBy(new SortCriteria(orderBy));
        }

        /// <summary>
        /// Continuation order query (then by) by matching property string name <paramref name="orderBy"/>
        /// against the custom type <typeparamref name="T"/> using the specified direction.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderBy"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string orderBy, SortDirectionOption direction)
        {
            return source.ThenBy(new SortCriteria(orderBy, direction));
        }

        /// <summary>
        /// Continuation order query (then by) based on provided sort info. 
        /// Uses reflection and expression trees to dynamically generate the OrderBy statement.
        /// Reference - http://stackoverflow.com/a/233505
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, SortCriteria sort)
        {
            if (sort == null)
                return source;

            return (IOrderedQueryable<T>)InvokeSortingOperation(source, sort, continuation: true);
        }

        /// <summary>
        /// Order list by matching property string name <paramref name="orderBy"/>
        /// against the custom type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> source, string orderBy)
        {
            return source.OrderBy(new SortCriteria(orderBy));
        }

        /// <summary>
        /// Order list by matching property string name <paramref name="orderBy"/>
        /// against the custom type <typeparamref name="T"/>. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderBy"></param>
        /// <param name="direction">Sort direction.</param>
        /// <returns></returns>
        public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> source, string orderBy, SortDirectionOption direction)
        {
            return source.OrderBy(new SortCriteria(orderBy, direction));
        }

        /// <summary>
        /// Order list based on provided sort info. 
        /// Uses reflection and expression trees to dynamically generate the OrderBy statement.
        /// Reference - http://stackoverflow.com/a/233505 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> source, SortCriteria sort)
        {
            if (sort == null)
                return source.OrderBy(x => x);

            return (IOrderedEnumerable<T>)InvokeSortingOperation(source, sort, continuation: false);
        }

        /// <summary>
        /// Continuation order query (then by) by matching property string name <paramref name="orderBy"/>
        /// against the custom type <typeparamref name="T"/>. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public static IOrderedEnumerable<T> ThenBy<T>(this IOrderedEnumerable<T> source, string orderBy)
        {
            return source.ThenBy(new SortCriteria(orderBy));
        }

        /// <summary>
        /// Continuation order query (then by) by matching property string name <paramref name="orderBy"/>
        /// against the custom type <typeparamref name="T"/> using the specified direction.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderBy"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static IOrderedEnumerable<T> ThenBy<T>(this IOrderedEnumerable<T> source, string orderBy, SortDirectionOption direction)
        {
            return source.ThenBy(new SortCriteria(orderBy, direction));
        }

        /// <summary>
        /// Continuation order query (then by) based on provided sort info. 
        /// Uses reflection and expression trees to dynamically generate the OrderBy statement.
        /// Reference - http://stackoverflow.com/a/233505
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public static IOrderedEnumerable<T> ThenBy<T>(this IOrderedEnumerable<T> source, SortCriteria sort)
        {
            if (sort == null)
                return source;

            return (IOrderedEnumerable<T>)InvokeSortingOperation(source, sort, continuation: true);
        }

        /// <summary>
        /// Extends <see cref="Queryable.OrderBy{TSource, TKey}(IQueryable{TSource}, Expression{Func{TSource, TKey}})"/>
        /// and <see cref="Queryable.OrderByDescending{TSource, TKey}(IQueryable{TSource}, Expression{Func{TSource, TKey}})"/>
        /// based on dynamic order direction <paramref name="direction"/>.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, SortDirectionOption direction)
        {
            switch (direction)
            {
                case SortDirectionOption.Ascending:
                    return source.OrderBy(keySelector);
                case SortDirectionOption.Descending:
                    return source.OrderByDescending(keySelector);
                default:
                    throw new UnsupportedEnumException(direction);
            }
        }

        /// <summary>
        /// Extends <see cref="Queryable.ThenBy{TSource, TKey}(IOrderedQueryable{TSource}, Expression{Func{TSource, TKey}})"/>
        /// and <see cref="Queryable.ThenByDescending{TSource, TKey}(IOrderedQueryable{TSource}, Expression{Func{TSource, TKey}})"/>
        /// based on dynamic order direction <paramref name="direction"/>.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static IOrderedQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, SortDirectionOption direction)
        {
            switch (direction)
            {
                case SortDirectionOption.Ascending:
                    return source.ThenBy(keySelector);
                case SortDirectionOption.Descending:
                    return source.ThenByDescending(keySelector);
                default:
                    throw new InvalidOperationException("ThenBy SortDirection not valid.");
            }
        }
    }
}
