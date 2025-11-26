using Common.Core.Validation;

namespace Common.Core.Domain
{
    public class ResultListFilter : ValueObject<ResultListFilter>
    {
        public static readonly ResultListFilter Default = new ResultListFilter(PageCriteria.Default, SortCriteria.DefaultByName);

        public ResultListFilter(
            int page, 
            int pageSize, 
            string sortProperty, 
            string sortDirection = "Ascending") 
            : this(new PageCriteria(pageSize, page), new SortCriteria(sortProperty, sortDirection))
        {

        }

        public ResultListFilter(PageCriteria paging, SortCriteria sorting)
        {
            Guard.IsNotNull(paging, nameof(paging));
            Guard.IsNotNull(sorting, nameof(sorting));

            Paging = paging;
            Sorting = sorting;
        }

        public PageCriteria Paging { get; private set; }
        public SortCriteria Sorting { get; private set; }
    }
}
