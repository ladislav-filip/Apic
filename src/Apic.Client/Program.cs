using System;
using System.Threading;
using System.Threading.Tasks;
using Apic.Contracts.Customers;
using Refit;

namespace Apic.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var gitHubApi = RestService.For<IApicApi>("https://localhost:44342");
            
            var customers = await gitHubApi.GetCustomers(new CustomerFilter {Page = 1, PageSize = 2});
            
            // var customer = await gitHubApi.GetCustomer(1)
        }
    }
}
