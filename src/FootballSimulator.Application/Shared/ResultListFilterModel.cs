using Common.Core;
using Common.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace FootballSimulator.Application.Models
{
    [Serializable]
    public class ResultListFilterModel
    {
        public const string BindProperty = $"{nameof(PageSize)}," +
            $"{nameof(CurrentPage)},{nameof(SortProperty)},{nameof(SortDirection)}";

        public int PageSize { get; set; }

        public int CurrentPage { get; set; }

        [MaxLength(300)]
        public string? SortProperty { get; set; }
        [MaxLength(50)]
        public string? SortDirection { get; set; }

        public static ResultListFilterModel Default
        {
            get
            {
                var filter = new ResultListFilter(PageCriteria.Default, SortCriteria.DefaultById);

                return new ResultListFilterModel()
                {
                    CurrentPage = filter.Paging.Current,
                    PageSize = filter.Paging.Size,
                    SortProperty = filter.Sorting.SortBy,
                    SortDirection = filter.Sorting.Direction.AsFriendlyName()
                };
            }
        }

        public ResultListFilter FilterValue => new(CurrentPage, PageSize, SortProperty, SortDirection);

    }
}