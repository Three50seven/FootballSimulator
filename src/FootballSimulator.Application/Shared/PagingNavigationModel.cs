using Common.Core;
using Common.Core.Domain;
using Common.Core.Validation;

namespace FootballSimulator.Application.Models
{
    [Serializable]
    public class PagingNavigationModel
    {
        private const int _maxNumOfPagesToDisplay = 3; //num of pages allowed to display in pager
        private IEnumerable<SelectItem> numPerPageOptions;

        public PagingNavigationModel() //default constructor here for mapping values
            : this(PageCriteria.DefaultPageSizeOptions)
        {

        }

        public PagingNavigationModel(int[] numPerPageOptions)
        {
            if (numPerPageOptions != null && numPerPageOptions.Length > 0)
                NumPerPageOptions = numPerPageOptions.ToSelectList();
        }

        public PagingNavigationModel(PageCriteria paging, int totalCount) : this(paging, totalCount, null)
        {

        }

        public PagingNavigationModel(PageCriteria paging, int totalCount, int[] numPerPageOptions)
        {
            Guard.IsNotNull(paging, nameof(paging));
            Guard.IsPositive(totalCount, nameof(totalCount));

            Paging = paging;
            TotalCount = totalCount;

            if (numPerPageOptions != null && numPerPageOptions.Length > 0)
                NumPerPageOptions = numPerPageOptions.ToSelectList();
        }

        public int TotalCount { get; set; }

        public PageCriteria Paging { get; set; } = PageCriteria.Default;

        public virtual string CurrentStatus => $"{Paging.StartIndex + 1} - {(CurrentIsMaxPage ? TotalCount : Paging.EndIndex)} of {TotalCount}.";
        public int PageCount => Paging.PageCount(TotalCount);
        public int MaxPageIndex => PageCount - 1;
        public bool CurrentIsMinPage => Paging.CurrentIndex == 0;
        public bool CurrentIsMaxPage => Paging.CurrentIndex == MaxPageIndex;
        public bool MinPageDisplaying => ActivePages.Any() ? ActivePages.First().PageNumber == 1 : false;
        public bool MaxPageDisplaying => ActivePages.Any() ? ActivePages.Last().PageNumber == PageCount : false;

        public IEnumerable<PageSelectItem> AllPages
        {
            get
            {
                var pages = new List<PageSelectItem>();
                var maxIndex = MaxPageIndex;
                maxIndex = maxIndex >= 0 ? maxIndex : 0;

                for (var i = 0; i <= maxIndex; i++)
                {
                    pages.Add(new PageSelectItem(i));
                }

                return pages;
            }
        }

        public IEnumerable<PageSelectItem> ActivePages
        {
            get
            {
                int minPage, maxPage;
                CalculateMinMax(out minPage, out maxPage);
                return AllPages.Where(x => x.PageNumber >= minPage && x.PageNumber <= maxPage);
            }
        }

        public IEnumerable<SelectItem> NumPerPageOptions
        {
            get
            {
                if (!numPerPageOptions.HasItems())
                    numPerPageOptions = PageCriteria.DefaultPageSizeOptions.Select(x => new SelectItem(x));

                return numPerPageOptions;
            }
            private set { numPerPageOptions = value; }
        }

        protected void CalculateMinMax(out int minPage, out int maxPage)
        {
            var radius = _maxNumOfPagesToDisplay / 2;
            int radiusLeft;
            int radiusRight;

            if (radius % 1 > 0) //radius is odd
            {
                //simply round down the radius for both left and right sides
                int rounded = (int)Math.Floor((decimal)radius);
                radiusLeft = rounded;
                radiusRight = rounded;
            }
            else //radius is even
            {
                radiusLeft = radius - 1;
                radiusRight = radius;
            }

            //find left and right page boundaries
            minPage = Paging.Current - radiusLeft;
            maxPage = Paging.Current + radiusRight;

            //validate and adjust min based on zero
            if (minPage <= 0)
            {
                maxPage = maxPage + Math.Abs(minPage) + 1;
                minPage = 1;
            }

            //validate and adjust max based on max allowed
            var maxPageAllowed = MaxPageIndex + 1;
            if (maxPage > maxPageAllowed)
            {
                minPage = (maxPageAllowed - _maxNumOfPagesToDisplay) + 1;
                maxPage = maxPageAllowed;
            }
        }
    }
}