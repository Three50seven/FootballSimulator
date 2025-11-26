using Common.Core.Domain;
using Common.Core.Validation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Core
{
    public static class PagedTask
    {
        /// <summary>
        /// Paging handler with per page callback and loop control.
        /// </summary>
        /// <param name="perPageCallback">Action callback on a new page based on the <paramref name="totalCount"/> and <paramref name="batchSize"/>.</param>
        /// <param name="totalCount">Total count to reach as a result of the paging loop.</param>
        /// <param name="batchSize">Batch/page size to take/skip as part of the paging process.</param>
        /// <param name="cancellationToken">Optional cancellation token. Token is checked at the top of each page start.</param>
        public static void Batched(
            Action<PageCriteria> perPageCallback,
            int totalCount,
            int batchSize = 100,
            CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(perPageCallback, nameof(perPageCallback));
            Guard.IsPositive(totalCount, nameof(totalCount));
            Guard.IsPositive(batchSize, nameof(batchSize));

            int page = 1;
            PageCriteria paging;

            do
            {
                cancellationToken.ThrowIfCancellationRequested();

                paging = new PageCriteria(batchSize, page);

                perPageCallback(paging);

                page++;
            }
            while (paging.SizeAll < totalCount);
        }

        /// <summary>
        /// Async paging handler with per page callback and loop control.
        /// </summary>
        /// <param name="perPageCallback">Async action callback on a new page based on the <paramref name="totalCount"/> and <paramref name="batchSize"/>.</param>
        /// <param name="totalCount">Total count to reach as a result of the paging loop.</param>
        /// <param name="batchSize">Batch/page size to take/skip as part of the paging process.</param>
        /// <param name="cancellationToken">Optional cancellation token. Token is checked at the top of each page start.</param>
        /// <returns></returns>
        public static async Task BatchedAsync(
            Func<PageCriteria, Task> perPageCallback,
            int totalCount,
            int batchSize = 100,
            CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(perPageCallback, nameof(perPageCallback));
            Guard.IsPositive(totalCount, nameof(totalCount));
            Guard.IsPositive(batchSize, nameof(batchSize));

            int page = 1;
            PageCriteria paging;

            do
            {
                cancellationToken.ThrowIfCancellationRequested();

                paging = new PageCriteria(batchSize, page);

                await perPageCallback(paging);

                page++;
            }
            while (paging.SizeAll < totalCount);
        }
    }
}
