using Apic.Contracts.Infrastructure.Transfer.Filters;
using System.Collections.Generic;

namespace Apic.Contracts.Infrastructure.Transfer
{
    /// <summary>
    /// Collection with paging information
    /// </summary>
    /// <typeparam name="T">Type of generic collection</typeparam>
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

        /// <summary>
        /// List of items
        /// </summary>
        public List<T> Items { get; set; }
        
        /// <summary>
        /// Total available items count
        /// </summary>
        public int TotalItems { get; set; }
        
        /// <summary>
        /// Size of result
        /// </summary>
        public int PageSize { get; set; }
        
        /// <summary>
        /// Total pages based otn current page size and total available items
        /// </summary>
        public int Page { get; set; }
    }
}
