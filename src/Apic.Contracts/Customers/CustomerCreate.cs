using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apic.Contracts.Customers
{
	public class CustomerCreate : IValidatableObject
	{
	    [Required] 
        [StringLength(100)]
		public string FirstName { get; set; }

	    [Required]
        [StringLength(100)]
		public string LastName { get; set; }

	    [Required]
        [EmailAddress]
		public string Email { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (!Email.EndsWith("@gmail.com"))
			{
				yield return new ValidationResult("Only @gmail.com domain is supported", new[] {nameof(Email)});
			}
		}
	}
}
