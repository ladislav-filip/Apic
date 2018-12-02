using System.ComponentModel.DataAnnotations;

namespace Apic.Contracts.Customers
{
	public class CustomerUpdate
	{
		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }
	}
}