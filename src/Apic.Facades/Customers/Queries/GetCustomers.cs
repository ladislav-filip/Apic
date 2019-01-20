using Apic.Common.Extensions;
using Apic.Contracts.Customers;
using Apic.Data.Context;
using Apic.Facades.Infrastructure;
using System;
using System.Linq;
using CustomerDbo = Apic.Entities.Customers.Customer;

namespace Apic.Facades.Customers.Queries
{
    public class GetCustomerQuery : IQuery<CustomerDbo>
    {
        private readonly ApicDbContext db;
        private readonly CustomerFilter filter;

        public GetCustomerQuery(ApicDbContext db, CustomerFilter filter)
        {
            this.db = db;
            this.filter = filter;
        }

        public IQueryable<CustomerDbo> Query()
        {
            var query = QueryBase().Skip(filter.PageSize * (filter.Page - 1)).Take(filter.PageSize);

            return query;
        }

        public int Count()
        {
            return QueryBase().Count();
        }

        private IQueryable<CustomerDbo> QueryBase()
        {
            var query = db.Set<CustomerDbo>().AsQueryable();

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
    }
}
