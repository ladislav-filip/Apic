using Apic.Contracts.Infrastructure.Transfer.Filters;
using System.Collections.Generic;

namespace Apic.Contracts.Infrastructure.Transfer
{
    public class Collection<T>
	{
        public Collection(List<T> items, int totalItems, PageFilter pageFilter)
        {
            Items = items;
            TotalItems = totalItems;
            PageSize = pageFilter.PageSize;
            Page = pageFilter.Page;
        }

		public List<T> Items { get; private set; }
		public int TotalItems { get; private set; }
		public int PageSize { get; private set; }
		public int Page { get; private set; }
	}
}
