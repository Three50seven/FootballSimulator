namespace Common.Core.Domain
{
    public class PageSelectItem : SelectItem
    {
        public PageSelectItem(int index) 
            : base((index + 1), (index + 1).ToString())
        {
            PageNumber = index + 1;
        }

        public int PageNumber { get; private set; }
    }
}
