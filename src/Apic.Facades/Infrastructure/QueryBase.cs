using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Apic.Common.Extensions;
using Apic.Contracts.Infrastructure.Transfer.Filters;
using Apic.Data.Context;

namespace Apic.Facades.Infrastructure
{
    public abstract class QueryBase<T, TS> : IQuery<T> where TS: BaseFilter where T : class
    {
        protected abstract Dictionary<string, string> PropertiesMap { get; }

        protected readonly IUnitOfWork Db;
        protected readonly TS Filter;

        protected QueryBase(IUnitOfWork db, TS filter)
        {
            this.Db = db;
            this.Filter = filter;
        }

        public virtual IQueryable<T> Build()
        {
            IQueryable<T> query = Query();
            string orderRules = GetMappedOrderByClause();
            if (orderRules.IsNotNullOrEmpty())
            {
                query = query.OrderBy(orderRules);
            }

            PageFilter pageFilter = Filter as PageFilter;
            if (pageFilter != null)
            {
                query = query.Skip(pageFilter.PageSize * (pageFilter.Page - 1)).Take(pageFilter.PageSize);
            }

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
            if (Filter.Sort.IsNullOrEmpty())
            {
                return string.Empty;
            }

            Dictionary<string, string> mappedRules = new Dictionary<string, string>();
            foreach (var orderRule in Filter.OrderByRules())
            {
                string contractProperty = PropertiesMap.FirstOrDefault(x => x.Key.Equals(orderRule.Key, StringComparison.InvariantCultureIgnoreCase)).Value;
                if (contractProperty.IsNotNullOrEmpty())
                {
                    mappedRules.Add(contractProperty, orderRule.Value);
                }
            }

            return string.Join(", ", mappedRules.Select(x => x.Key + " " + x.Value));
        }
    }
}
