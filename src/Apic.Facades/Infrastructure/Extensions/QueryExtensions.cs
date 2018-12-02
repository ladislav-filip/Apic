using System.Linq;
using Apic.Contracts.Infrastructure.Transfer.Filters;
using Apic.Data.Infrastructure;

namespace Apic.Facades.Infrastructure.Extensions
{
	public static class QueryableExtensions
	{
		public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, PageFilter pageFilter)
		{
			return query.Skip((pageFilter.Page - 1) * pageFilter.PageSize).Take(pageFilter.PageSize);
		}

		public static IQueryable<T> ExecuteQuery<T>(this IQueryable<T> queryable, IQuery<T> query)
		{
			return query.Execute(queryable);
		}
	}
}
