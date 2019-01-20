using Apic.Common.Exceptions;
using Apic.Data.Context;
using Apic.Entities.Customers;
using System;
using System.Linq;

namespace Apic.Data.Repositories
{
    public class CustomerRepository
    {
        private readonly ApicDbContext db;

        public Customer GetSingle(int customerId)
        {
            try
            {
                return db.Set<Customer>().Single(x => x.Id == customerId);
            }
            catch (InvalidOperationException exception) when (exception.Message.Contains("Sequence contains"))
            {
                throw new ObjectNotFoundException();
            }
        }
    }
}
