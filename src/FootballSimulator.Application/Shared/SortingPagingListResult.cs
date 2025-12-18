using Common.Core.Domain;

namespace FootballSimulator.Application.Models
{
    public class SortingPagingListResult
    {
        public SortCriteria? Sorting { get; set; }
        public PagingNavigationModel? Paging { get; set; }
    }
}
