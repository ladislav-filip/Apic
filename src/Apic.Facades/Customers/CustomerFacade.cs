using System.Collections.Generic;
using System.Threading.Tasks;
using Apic.Common.Attributes;
using Apic.Common.Exceptions;
using Apic.Contracts.Customers;
using Apic.Contracts.Infrastructure.Transfer;
using Apic.Data.Context;
using Apic.Data.Repositories;
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
		private readonly IUnitOfWork uow;
		private readonly IMapper mapper;
        private readonly ModelStateAccessor requestState;

		public CustomerFacade(IUnitOfWork uow, IMapper mapper, ModelStateAccessor requestState)
		{
			this.uow = uow;
			this.mapper = mapper;
            this.requestState = requestState;
        }

		public async Task<Collection<Customer>> Get(CustomerFilter customerFilter)
		{
            GetCustomersQuery query = new GetCustomersQuery(uow, customerFilter);
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
            CustomerDbo customer = await uow.Customers.GetSingle(customerId);
            Customer customerResult = mapper.Map<Customer>(customer);
			return customerResult;
		}

		public async Task<Customer> Create(CustomerCreate createRequest)
		{
			CustomerDbo customer = mapper.Map<CustomerDbo>(createRequest);

			uow.Add(customer);
			await uow.SaveChangesAsync();

			Customer result = mapper.Map<Customer>(customer);

			return result;
		}

		public async Task<Customer> Update(int id, CustomerUpdate model)
		{
            CustomerDbo customer = await uow.Customers.GetSingle(id);
            customer = mapper.Map(model, customer);

			await uow.SaveChangesAsync();
			Customer result = mapper.Map<Customer>(customer);

			return result;
		}

		public async Task Delete(int customerId)
		{
			bool existsCustomer = await uow.Set<Customer>().AnyAsync(x => x.Id == customerId);
			if (!existsCustomer)
			{
                throw new ObjectNotFoundException("Customer was not found");
			}

            uow.Remove(new CustomerDbo { Id = customerId });
			await uow.SaveChangesAsync();
		}
	}
}
