using Apic.Contracts.Infrastructure.Transfer.Filters;
using System.ComponentModel.DataAnnotations;

namespace Apic.Contracts.Customers
{
	public class CustomerFilter : PageFilter
	{
		public CustomerFilter()
		{
			MaxPageSize = 100;
		}

        public string LastName { get; set; }
        public string Domain { get; set; }
	}
}
