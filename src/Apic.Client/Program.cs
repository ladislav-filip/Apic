using System;
using System.Threading;
using System.Threading.Tasks;
using Apic.Contracts.Customers;
using Apic.Contracts.Infrastructure.Transfer;
using Refit;

namespace Apic.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var gitHubApi = RestService.For<IApicApi>("https://localhost:44342");
            
            Collection<Customer> customers = await gitHubApi.GetCustomers(new CustomerFilter {Page = 1, PageSize = 2});
            
            // var customer = await gitHubApi.GetCustomer(1)
        }
    }
}
