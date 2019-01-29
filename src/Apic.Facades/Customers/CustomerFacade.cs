using System.Collections.Generic;
using System.Threading.Tasks;
using Apic.Common.Attributes;
using Apic.Common.Exceptions;
using Apic.Contracts.Customers;
using Apic.Contracts.Infrastructure.Transfer;
using Apic.Data.Context;
using Apic.Facades.Customers.Queries;
using Apic.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using CustomerDbo = Apic.Entities.Customers.Customer;

namespace Apic.Facades.Customers
{
    [ScopedService]
	public class CustomerFacade : ICustomerFacade
	{
		private readonly ApicDbContext dbContext;
		private readonly IMapper mapper;
        private readonly ModelStateAccessor requestState;

		public CustomerFacade(ApicDbContext dbContext, IMapper mapper, ModelStateAccessor requestState)
		{
			this.dbContext = dbContext;
			this.mapper = mapper;
            this.requestState = requestState;
		}

		public async Task<Collection<Customer>> Get(CustomerFilter customerFilter)
		{
            GetCustomersQuery query = new GetCustomersQuery(dbContext, customerFilter);
            List<Customer> items = await mapper.ProjectTo<Customer>(query.Build()).ToListAsync();

            var result = new Collection<Customer>(items, query.Count(), customerFilter);

            // demo purpose only
		    if (items.Count < result.TotalItems)
		    {
                requestState.Messages.Add("Use pagination for showing all records.");
		    }

		    return result;
		}

        public async Task<Customer> Get(int customerId)
		{
			CustomerDbo customer = await dbContext.Customers.FirstOrDefaultAsync(x => x.Id == customerId);
            if (customer == null)
            {
                throw new ObjectNotFoundException("Customer was not found!");
            }

            Customer customerResult = mapper.Map<Customer>(customer);
			return customerResult;
		}

		public async Task<Customer> Create(CustomerCreate createRequest)
		{
			CustomerDbo customer = mapper.Map<CustomerDbo>(createRequest);

			dbContext.Customers.Add(customer);
			await dbContext.SaveChangesAsync();

			Customer result = mapper.Map<Customer>(customer);

			return result;
		}

		public async Task<Customer> Update(int id, CustomerUpdate model)
		{
			CustomerDbo customer = await dbContext.Customers.FirstOrDefaultAsync(x => x.Id == id);
			customer = mapper.Map(model, customer);

			await dbContext.SaveChangesAsync();
			Customer result = mapper.Map<Customer>(customer);

			return result;
		}

		public async Task Delete(int customerId)
		{
			bool existsCustomer = await dbContext.Customers.AnyAsync(x => x.Id == customerId);
			if (!existsCustomer)
			{
                throw new ObjectNotFoundException("Customer was not found");
			}

            dbContext.Customers.Remove(new CustomerDbo { Id = customerId });
			await dbContext.SaveChangesAsync();
		}
	}
}
