namespace Common.Core.Domain
{
    public class SortCriteria : ValueObject<SortCriteria>
    {
        public SortCriteria(string sortBy, SortDirectionOption direction)
        {
            SortBy = string.IsNullOrWhiteSpace(sortBy) ? "Id" : sortBy.Trim();
            Direction = direction;
        }

        public SortCriteria(string sortBy, string dir = "asc") :
            this(sortBy, dir == "Descending" || dir == "desc" ? SortDirectionOption.Descending : SortDirectionOption.Ascending)
        {

        }

        public string SortBy { get; private set; }

        public SortDirectionOption Direction { get; private set; }

        public string DirectionAbbr => Direction == SortDirectionOption.Ascending ? "asc" : "desc";

        public string QuerySortOperation => GetQuerySortOperation();

        public static SortCriteria DefaultByName => new SortCriteria("Name");
        public static SortCriteria DefaultById => new SortCriteria("Id");

        public string GetQuerySortOperation(bool continuation = false)
        {
            if (continuation)
                return Direction == SortDirectionOption.Ascending ? "ThenBy" : "ThenByDescending";
            else
                return Direction == SortDirectionOption.Ascending ? "OrderBy" : "OrderByDescending";
        }
    }
}
