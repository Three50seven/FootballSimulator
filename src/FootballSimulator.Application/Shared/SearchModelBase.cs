using Common.Core.Domain;

namespace FootballSimulator.Application.Models
{
    public abstract class SearchModelBase<T>
    {
        public const string BindProperty = nameof(Results) + ","
                                            + nameof(Sorting) + ","
                                            + nameof(Paging) + ","
                                            + nameof(SearchPerformed) + ","
                                            + nameof(FilterChanged) + ","
                                            + nameof(SearchCount);

        public IEnumerable<T> Results { get; set; } = new List<T>();
        public SortCriteria Sorting { get; set; } = SortCriteria.DefaultById;
        public PagingNavigationModel Paging { get; set; } = new PagingNavigationModel();
        public bool SearchPerformed { get; set; }
        public bool FilterChanged { get; set; }
        public int SearchCount { get; set; }
    }
}
