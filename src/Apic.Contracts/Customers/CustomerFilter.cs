using Apic.Contracts.Infrastructure.Transfer.Filters;

namespace Apic.Contracts.Customers
{
	public class CustomerFilter : PageFilter
	{
		public CustomerFilter()
		{
			MaxPageSize = 100;
		}

	    public override string Sort { get; set; } = "Id";
	    public string LastName { get; set; }
        public string Domain { get; set; }
	}
}
