namespace FootballSimulator.Application.Models
{
    public class SearchQueryModel<TFilter>
    {
        public const string BindProperty = $"{nameof(Filter)},{nameof(ResultFilter)}";
        public TFilter? Filter { get; set; }
        public ResultListFilterModel? ResultFilter { get; set; }
    }
}
