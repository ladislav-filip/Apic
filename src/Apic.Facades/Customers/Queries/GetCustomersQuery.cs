using System.Linq;
using Apic.Contracts.Customers;
using Apic.Contracts.Infrastructure.Transfer.Filters;
using Apic.Data.Context;
using Apic.Data.Infrastructure;
using Customer = Apic.Entities.Customers.Customer;

namespace Apic.Facades.Customers.Queries
{

	public class GetCustomersQuery : IQuery<Customer>
	{
		public GetCustomersQuery(CustomerFilter customerFilter)
		{
			//this.Name = customerFilter.Name;
		}

		//public string Name { get; set; }

		public IQueryable<Customer> Execute(IQueryable<Customer> queryable)
		{
			return queryable.Where(x => x.FirstName != null).OrderByDescending(x => x.Id);
		}
	}
}
