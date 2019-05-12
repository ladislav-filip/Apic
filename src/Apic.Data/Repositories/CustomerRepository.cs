using Apic.Common.Exceptions;
using Apic.Data.Context;
using Apic.Entities.Customers;
using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Apic.Common.Attributes;
using Microsoft.EntityFrameworkCore;

namespace Apic.Data.Repositories
{
    [ScopedService]
    public class CustomerRepository : BaseRepository<Customer, int>
    {
        private readonly DbSet<Customer> set;

        public CustomerRepository(DbSet<Customer> set) : base(set)
        {
            this.set = set;
        }

        public async Task<Customer> GetSingle(int customerId)
        {
            try
            {
                return await set.SingleAsync(x => x.Id == customerId);
            }
            catch (InvalidOperationException exception) when (exception.Message.Contains("contain any elements"))
            {
                throw new ObjectNotFoundException("Customer has not been found!");
            }
        }
        
    }
}
