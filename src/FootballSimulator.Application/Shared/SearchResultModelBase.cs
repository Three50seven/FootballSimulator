namespace FootballSimulator.Application.Models
{
    public class SearchResultModelBase<T> : SortingPagingListResult
    {
        public IEnumerable<T> Results { get; set; } = [];
    }   
}
