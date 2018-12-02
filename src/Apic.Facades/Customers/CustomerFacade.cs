using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apic.Common.Attributes;
using Apic.Contracts.Customers;
using Apic.Contracts.Infrastructure.Transfer;
using Apic.Data.Context;
using Apic.Facades.Customers.Queries;
using Apic.Facades.Infrastructure.Extensions;
using Apic.Facades.Mappers;
using Microsoft.EntityFrameworkCore;
using CustomerDbo = Apic.Entities.Customers.Customer;

namespace Apic.Facades.Customers
{
	public interface ICustomerFacade
	{
		Task<DataListResult<Customer>> Get(CustomerFilter customerFilter);
		Task<DataResult<Customer>> Get(int customerId);
		Task<DataResult<Customer>> Create(CustomerCreate createRequest);
		Task<DataResult<Customer>> Update(int id, CustomerUpdate model);
		Task<Result> Delete(int customerId);
	}

	[Service]
	public class CustomerFacade : ICustomerFacade
	{
		private readonly ApicDbContext dbContext;
		private readonly ICustomerMapper customerMapper;

		public CustomerFacade(ApicDbContext dbContext, ICustomerMapper customerMapper)
		{
			this.dbContext = dbContext;
			this.customerMapper = customerMapper;
		}

		public async Task<DataListResult<Customer>> Get(CustomerFilter customerFilter)
		{
			IQueryable<CustomerDbo> query = dbContext.Customers.ExecuteQuery(new GetCustomersQuery(customerFilter));
			List<Customer> result = await customerMapper.Map(query.ApplyPaging(customerFilter));
			return DataListResult<Customer>.Ok(result, customerFilter, query.Count());

		}

		public async Task<DataResult<Customer>> Get(int customerId)
		{
			CustomerDbo customer = await dbContext.Customers.FirstOrDefaultAsync(x => x.Id == customerId);
			if (customer == null)
			{
				return DataResult<Customer>.NotFound();
			}

			Customer customerResult = customerMapper.Map(customer);
			return DataResult<Customer>.Ok(customerResult);
		}

		public async Task<DataResult<Customer>> Create(CustomerCreate createRequest)
		{
			CustomerDbo customer = customerMapper.Map(createRequest);

			dbContext.Customers.Add(customer);
			await dbContext.SaveChangesAsync();

			Customer result = customerMapper.Map(customer);

			return DataResult<Customer>.Ok(result);
		}

		public async Task<DataResult<Customer>> Update(int id, CustomerUpdate model)
		{
			CustomerDbo customer = await dbContext.Customers.FirstOrDefaultAsync(x => x.Id == id);
			customer = customerMapper.Map(model, customer);

			await dbContext.SaveChangesAsync();
			Customer result = customerMapper.Map(customer);

			return DataResult<Customer>.Ok(result);
		}

		public async Task<Result> Delete(int customerId)
		{
			bool existsCustomer = await dbContext.Customers.AnyAsync(x => x.Id == customerId);
			if (!existsCustomer)
			{
				return Result.NotFound();
			}

			dbContext.Customers.Remove(new CustomerDbo{Id = customerId});
			await dbContext.SaveChangesAsync();

			return Result.Ok();
		}
	}
}
