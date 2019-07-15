using System.Threading.Tasks;
using Apic.Contracts.Customers;
using Apic.Contracts.Infrastructure.Transfer;
using Refit;

namespace Apic.Client
{
    public interface IApicApi
    {
        [Get("/api/customers")]
        Task<Result<Collection<Customer>>> GetCustomers(CustomerFilter filter);
        
        [Get("/api/customers/{id}")]
        Task<Result<Customer>> GetCustomer(int id);
    }
}