using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apic.Contracts.Infrastructure.Transfer.Filters
{
	public class PageFilter : IValidatableObject
	{
		protected int MaxPageSize = 100;

		public const int DefaultPage = 1;
		public const int DefaultPageSize = 10;

		public int PageSize { get; set; } = DefaultPageSize;
		public int Page { get; set; } = DefaultPage;

		public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (PageSize > MaxPageSize)
			{
				PageSize = MaxPageSize;
			}

			yield break;
		}
	}
}
