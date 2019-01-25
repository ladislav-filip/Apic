using Apic.Common.Extensions;
using Apic.Contracts.Customers;
using Apic.Data.Context;
using Apic.Facades.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using CustomerDbo = Apic.Entities.Customers.Customer;

namespace Apic.Facades.Customers.Queries
{
    public class GetCustomersQuery : IQuery<CustomerDbo>
    {
        private readonly ApicDbContext db;
        private readonly CustomerFilter filter;

        private readonly Dictionary<string, string> propertiesMap;

        public GetCustomersQuery(ApicDbContext db, CustomerFilter filter)
        {
            this.db = db;
            this.filter = filter;

            propertiesMap = new Dictionary<string, string>()
            {
                { "Id", "Id" },
                { "Email", "Email" },
            };
        }

        public IQueryable<CustomerDbo> Query()
        {
            // order by
            IQueryable<CustomerDbo> query = QueryBase();
            string orderRules = GetMappedOrderByClause();
            if (orderRules.IsNotNullOrEmpty())
            {
                query = query.OrderBy(orderRules);
            }

            // paging
            query = query.Skip(filter.PageSize * (filter.Page - 1)).Take(filter.PageSize);

            return query;
        }

        public int Count()
        {
            return QueryBase().Count();
        }

        private IQueryable<CustomerDbo> QueryBase()
        {
            var query = db.Set<CustomerDbo>().AsQueryable();

            if (filter.SearchQuery.IsNotNullOrEmpty())
            {
                query = query.Where(x => x.FirstName.Contains(filter.SearchQuery) || x.LastName.Contains(filter.SearchQuery));
            }

            if (filter.Domain.IsNotNullOrEmpty())
            {
                query = query.Where(x => x.Email.EndsWith(filter.Domain, StringComparison.InvariantCultureIgnoreCase));
            }

            if (filter.LastName.IsNotNullOrEmpty())
            {
                query = query.Where(x => x.Email.Equals(filter.Domain, StringComparison.InvariantCultureIgnoreCase));
            }

            return query;
        }

        private string GetMappedOrderByClause()
        {
            if (StringExtensions.IsNullOrEmpty(filter.OrderBy))
            {
                return string.Empty;
            }

            Dictionary<string, string> mappedRules = new Dictionary<string, string>();
            foreach (var orderRule in filter.OrderByRules())
            {
                if (propertiesMap.ContainsKey(orderRule.Key))
                {
                    mappedRules.Add(propertiesMap[orderRule.Key], orderRule.Value);
                }
            }

            return string.Join(", ", mappedRules.Select(x => x.Key + " " + x.Value));
        }
    }
}
