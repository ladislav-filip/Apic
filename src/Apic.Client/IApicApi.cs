using System.Threading.Tasks;
using Apic.Contracts.Customers;
using Apic.Contracts.Infrastructure.Transfer;
using Refit;

namespace Apic.Client
{
    public interface IApicApi
    {
        [Get("/api/customers")]
        Task<Collection<Customer>> GetCustomers(CustomerFilter filter);
        
        [Get("/api/customers/{id}")]
        Task<Customer> GetCustomer(int id);
    }
}