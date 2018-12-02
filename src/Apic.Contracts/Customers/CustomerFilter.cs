using Apic.Contracts.Infrastructure.Transfer.Filters;

namespace Apic.Contracts.Customers
{
	public class CustomerFilter : PageFilter
	{
		public CustomerFilter()
		{
			MaxPageSize = 100;
		}
	}
}
