using System;
using System.Collections.Generic;
using System.Linq;
using Apic.Common.Extensions;
using Apic.Contracts.Customers;
using Apic.Data.Context;
using Apic.Facades.Infrastructure;
using Microsoft.EntityFrameworkCore;
using CustomerDbo = Apic.Entities.Customers.Customer;

namespace Apic.Facades.Customers.Queries
{
    public class GetCustomersQuery : QueryBase<CustomerDbo, CustomerFilter>
    {
        public GetCustomersQuery(IUnitOfWork db, CustomerFilter filter) : base(db, filter)
        {
        }

        protected override Dictionary<string, string> PropertiesMap => new Dictionary<string, string>()
        {
            { nameof(Customer.Id), nameof(CustomerDbo.Id) },
            { nameof(Customer.Email), nameof(CustomerDbo.Email) }
        };

        protected override IQueryable<CustomerDbo> Query()
        {
            var query = Db.Set<CustomerDbo>().AsQueryable();

            if (Filter.SearchQuery.IsNotNullOrEmpty())
            {
                query = query.Where(x => x.FirstName.Contains(Filter.SearchQuery) || x.LastName.Contains(Filter.SearchQuery));
            }

            if (Filter.Domain.IsNotNullOrEmpty())
            {
                query = query.Where(x => x.Email.EndsWith(Filter.Domain, StringComparison.InvariantCultureIgnoreCase));
            }

            if (Filter.LastName.IsNotNullOrEmpty())
            {
                query = query.Where(x => x.LastName.Equals(Filter.LastName, StringComparison.InvariantCultureIgnoreCase));
            }

            return query;
        }
    }
}
