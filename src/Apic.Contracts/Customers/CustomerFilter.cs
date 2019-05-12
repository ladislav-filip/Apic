using Apic.Contracts.Infrastructure.Transfer.Filters;

namespace Apic.Contracts.Customers
{
	public class CustomerFilter : PageFilter
	{
		public CustomerFilter()
		{
			MaxPageSize = 100;
		}

		/// <summary>
		/// Sort column name. Default is "Id". 
		/// </summary>
	    public override string Sort { get; set; } = "Id";
	    
		/// <summary>
		/// LastName starts with
		/// </summary>
		public string LastName { get; set; }
        public string Domain { get; set; }
	}
}
