using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Core.Domain
{
    public static class SelectItemExtensions
    {
        public static IEnumerable<int> ToIdList<T>(this IEnumerable<T> items) where T : SelectItem
        {
            return items.Where(x => x.Id.CleanForNull() != null).Select(x => (int)x.Id).ToList();
        }

        public static IList<SelectInputItem> ToInputList<T>(this IEnumerable<T> items) where T : SelectItem
        {
            return items.Select(x => new SelectInputItem(x.Value, x.Name, x.Selected)).ToList();
        }

        public static string LookupName<T>(this IEnumerable<T> items, string value) where T : SelectItem
        {
            return items.Where(x => string.Compare(x.Value, value, StringComparison.CurrentCultureIgnoreCase) == 0).Select(x => x.Name).FirstOrDefault();
        }

        public static IEnumerable<SelectItem> MarkSelected<T>(this IEnumerable<T> items, int[] selectedIds) where T : SelectItem
        {
            foreach (var item in items)
            {
                item.Selected = selectedIds.Contains(item.Id ?? 0);
            }

            return items;
        }
    }
}
