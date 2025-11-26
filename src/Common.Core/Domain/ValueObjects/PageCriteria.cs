using Common.Core.Validation;
using System;

namespace Common.Core.Domain
{
    public class PageCriteria : ValueObject<PageCriteria>
    {
        public static int DefaultPageSize = 25;
        public static int[] DefaultPageSizeOptions = new int[] { 25, 50, 100, 500 };

        public PageCriteria(int size, int current)
        {
            Guard.IsGreaterThanZero(size, nameof(size));
            Guard.IsGreaterThanZero(current, nameof(current));

            Size = size;
            Current = current;
        }

        public int Size { get; private set; }
        public int Current { get; private set; }

        public int Next => Current + 1;
        public int Prev => Current <= 1 ? 1 : Current - 1;
        public int CurrentIndex => (Current - 1);
        public int StartIndex => CurrentIndex * Size;
        public int EndIndex => StartIndex + Size;
        public int SizeAll => Current * Size;

        public int PageCount(int totalRecordCount)
        {
            if (Size <= 0 || totalRecordCount <= 0)
                return 0;

            // NOTE: the casts need to be here so the calculation is performed and rounded properly
            return (int)Math.Ceiling((double)totalRecordCount / (double)Size);
        }

        public static PageCriteria Default => new PageCriteria(DefaultPageSize, 1);
    }
}
