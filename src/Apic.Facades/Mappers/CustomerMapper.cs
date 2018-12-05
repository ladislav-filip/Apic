using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apic.Common.Attributes;
using Apic.Contracts.Customers;
using Microsoft.EntityFrameworkCore;
using CustomerDbo = Apic.Entities.Customers.Customer;

namespace Apic.Facades.Mappers
{
	public interface ICustomerMapper
	{
		Task<List<Customer>> Map(IQueryable<CustomerDbo> customers);
		CustomerDbo Map(CustomerCreate createRequest);
		CustomerDbo Map(CustomerUpdate createRequest, CustomerDbo origin);
		Customer Map(CustomerDbo customerDbo);
	}

	[ScopedService]
	public class CustomerMapper : ICustomerMapper
	{
		public async Task<List<Customer>> Map(IQueryable<CustomerDbo> customers)
		{
			var result = await customers.Select(x => Map(x)).ToListAsync();

			return result;
		}

		public CustomerDbo Map(CustomerCreate createRequest)
		{
			CustomerDbo origin = new CustomerDbo();

			origin.FirstName = createRequest.FirstName;
			origin.LastName = createRequest.LastName;
			origin.Email = createRequest.Email;

			return origin;
		}

		public CustomerDbo Map(CustomerUpdate updateRequest, CustomerDbo origin)
		{
			origin.FirstName = updateRequest.FirstName;
			origin.LastName = updateRequest.LastName;

			return origin;
		}

		public Customer Map(CustomerDbo customerDbo)
		{
			return new Customer()
			{
				Id = customerDbo.Id,
				FirstName = customerDbo.FirstName,
				LastName = customerDbo.LastName
			};
		}
	}
}
