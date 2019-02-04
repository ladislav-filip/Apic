using Apic.Common.Exceptions;
using Apic.Data.Context;
using Apic.Entities.Customers;
using System;
using System.Linq;
using System.Threading.Tasks;
using Apic.Common.Attributes;
using Microsoft.EntityFrameworkCore;

namespace Apic.Data.Repositories
{
    [ScopedService]
    public class CustomerRepository
    {
        private readonly ApicDbContext db;

        public CustomerRepository(ApicDbContext db)
        {
            this.db = db;
        }

        public async Task<Customer> GetSingle(int customerId)
        {
            try
            {
                return await db.Set<Customer>().SingleAsync(x => x.Id == customerId);
            }
            catch (InvalidOperationException exception) when (exception.Message.Contains("contain any elements"))
            {
                throw new ObjectNotFoundException("Customer has not been found!");
            }
        }
    }
}
