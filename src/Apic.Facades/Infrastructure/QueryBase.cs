using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Apic.Common.Extensions;
using Apic.Contracts.Infrastructure.Transfer.Filters;
using Apic.Data.Context;

namespace Apic.Facades.Infrastructure
{
    public abstract class QueryBase<T, TS> : IQuery<T> where TS: PageFilter where T : class
    {
        protected abstract Dictionary<string, string> PropertiesMap { get; }

        protected readonly ApicDbContext Db;
        protected readonly TS Filter;

        protected QueryBase(ApicDbContext db, TS filter)
        {
            this.Db = db;
            this.Filter = filter;
        }

        public virtual IQueryable<T> Build()
        {
            // order by
            IQueryable<T> query = Query();
            string orderRules = GetMappedOrderByClause();
            if (orderRules.IsNotNullOrEmpty())
            {
                query = query.OrderBy(orderRules);
            }

            // paging
            query = query.Skip(Filter.PageSize * (Filter.Page - 1)).Take(Filter.PageSize);

            return query;
        }

        public virtual int Count()
        {
            return Query().Count();
        }

        protected virtual IQueryable<T> Query()
        {
            var query = Db.Set<T>().AsQueryable();

            return query;
        }

        private string GetMappedOrderByClause()
        {
            if (Filter.OrderBy.IsNullOrEmpty())
            {
                return string.Empty;
            }

            Dictionary<string, string> mappedRules = new Dictionary<string, string>();
            foreach (var orderRule in Filter.OrderByRules())
            {
                if (PropertiesMap.ContainsKey(orderRule.Key))
                {
                    mappedRules.Add(PropertiesMap[orderRule.Key], orderRule.Value);
                }
            }

            return string.Join(", ", mappedRules.Select(x => x.Key + " " + x.Value));
        }
    }
}
