using Common.Core.Domain;
using Common.Core.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Core
{
    public static class PagingExtensions
    {
        /// <summary>
        /// Order query list and then page query list from the result filter value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="resultListFilter"></param>
        /// <returns></returns>
        public static IQueryable<T> SortAndPage<T>(this IQueryable<T> query, ResultListFilter resultListFilter)
        {
            Guard.IsNotNull(resultListFilter, nameof(resultListFilter));

            return SortAndPage<T>(query, resultListFilter.Sorting, resultListFilter.Paging);
        }

        /// <summary>
        /// Order query list and then page query list from the result filter value.
        /// Includes the total of all items not paged.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="resultListFilter"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static IQueryable<T> SortAndPage<T>(this IQueryable<T> query, ResultListFilter resultListFilter, out int total)
        {
            Guard.IsNotNull(resultListFilter, nameof(resultListFilter));

            return SortAndPage<T>(query, resultListFilter.Sorting, resultListFilter.Paging, out total);
        }

        /// <summary>
        /// Order query list by <paramref name="sort"/> and then page query list by <paramref name="paging"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="sort">Ordering info.</param>
        /// <param name="paging">Paging info.</param>
        /// <returns></returns>
        public static IQueryable<T> SortAndPage<T>(this IQueryable<T> query, SortCriteria sort, PageCriteria paging)
        {
            //simply sort and page
            return query.OrderBy(sort).Page(paging);
        }

        /// <summary>
        /// Order query list by <paramref name="sort"/> and then page query list by <paramref name="paging"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="sort">Ordering info.</param>
        /// <param name="paging">Paging info.</param>
        /// <param name="total">Total count from the query list before paging.</param>
        /// <returns></returns>
        public static IQueryable<T> SortAndPage<T>(this IQueryable<T> query, SortCriteria sort, PageCriteria paging, out int total)
        {
            return query.OrderBy(sort).Page(paging, out total);
        }

        /// <summary>
        /// Page query list based on <paramref name="paging"/> info.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        public static IQueryable<T> Page<T>(this IOrderedQueryable<T> query, PageCriteria paging)
        {
            if (!query.IsOrdered())
                throw new ArgumentException("Query must be ordered before paging.");
            return query.Skip(paging.StartIndex).Take(paging.Size);
        }

        /// <summary>
        /// Page query list based on <paramref name="paging"/> info. Returns total count from query prior to paging.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="paging">Paging info.</param>
        /// <param name="total">Total count of query prior to paging.</param>
        /// <returns></returns>
        public static IQueryable<T> Page<T>(this IOrderedQueryable<T> query, PageCriteria paging, out int total)
        {
            total = query.Count(); //grab count of total before paging

            return query.Page(paging);
        }

        /// <summary>
        /// Order query list and then page query list from the result filter value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="resultListFilter"></param>
        /// <returns></returns>
        public static IEnumerable<T> SortAndPage<T>(this IEnumerable<T> query, ResultListFilter resultListFilter)
        {
            Guard.IsNotNull(resultListFilter, nameof(resultListFilter));

            return SortAndPage<T>(query, resultListFilter.Sorting, resultListFilter.Paging);
        }

        /// <summary>
        /// Order query list and then page query list from the result filter value.
        /// Includes the total of all items not paged.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="resultListFilter"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static IEnumerable<T> SortAndPage<T>(this IEnumerable<T> query, ResultListFilter resultListFilter, out int total)
        {
            Guard.IsNotNull(resultListFilter, nameof(resultListFilter));

            return SortAndPage<T>(query, resultListFilter.Sorting, resultListFilter.Paging, out total);
        }

        /// <summary>
        /// Order query list by <paramref name="sort"/> and then page query list by <paramref name="paging"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="sort">Ordering info.</param>
        /// <param name="paging">Paging info.</param>
        /// <returns></returns>
        public static IEnumerable<T> SortAndPage<T>(this IEnumerable<T> query, SortCriteria sort, PageCriteria paging)
        {
            //simply sort and page
            return query.OrderBy(sort).Page(paging);
        }

        /// <summary>
        /// Order query list by <paramref name="sort"/> and then page query list by <paramref name="paging"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="sort">Ordering info.</param>
        /// <param name="paging">Paging info.</param>
        /// <param name="total">Total count from the query list before paging.</param>
        /// <returns></returns>
        public static IEnumerable<T> SortAndPage<T>(this IEnumerable<T> query, SortCriteria sort, PageCriteria paging, out int total)
        {
            return query.OrderBy(sort).Page(paging, out total);
        }

        /// <summary>
        /// Page query list based on <paramref name="paging"/> info.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        public static IEnumerable<T> Page<T>(this IEnumerable<T> query, PageCriteria paging)
        {
            return query.Skip(paging.StartIndex).Take(paging.Size);
        }

        /// <summary>
        /// Page query list based on <paramref name="paging"/> info. Returns total count from query prior to paging.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="paging">Paging info.</param>
        /// <param name="total">Total count of query prior to paging.</param>
        /// <returns></returns>
        public static IEnumerable<T> Page<T>(this IEnumerable<T> query, PageCriteria paging, out int total)
        {
            total = query.Count(); //grab count of total before paging

            return query.Page(paging);
        }
    }
}
