using System.Threading.Tasks;
using Apic.Contracts.Customers;
using Apic.Contracts.Infrastructure.Transfer;

namespace Apic.Facades.Customers
{
    public interface ICustomerFacade
    {
        Task<Collection<Customer>> Get(CustomerFilter customerFilter);
        Task<Customer> Get(int customerId);
        Task<Customer> Create(CustomerCreate createRequest);
        Task<Customer> Update(int id, CustomerUpdate model);
        Task Delete(int customerId);
    }
}