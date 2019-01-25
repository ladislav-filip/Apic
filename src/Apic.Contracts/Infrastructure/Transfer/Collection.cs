using Apic.Contracts.Infrastructure.Transfer.Filters;
using System.Collections.Generic;

namespace Apic.Contracts.Infrastructure.Transfer
{
    public class Collection<T>
    {
        public Collection()
        {
        }

        public Collection(List<T> items, int totalItems, PageFilter pageFilter)
        {
            Items = items;
            TotalItems = totalItems;
            PageSize = pageFilter.PageSize;
            Page = pageFilter.Page;
        }

        public List<T> Items { get; set; }
        public int TotalItems { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }
}
