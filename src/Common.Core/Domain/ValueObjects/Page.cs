namespace Common.Core.Domain
{
    public class Page
    {
        private int _size = PageCriteria.DefaultPageSize;

        public Page()
        {
            
        }
        public Page(int page, int size)
        {
            Number = page;
            Size = size;
        }
        public int Number { get; set; }
        public int Size
        {
            get => _size;
            set => _size = value > 0 ? value : PageCriteria.DefaultPageSize;
        }

    }
}
