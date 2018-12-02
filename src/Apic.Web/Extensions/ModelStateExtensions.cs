using System;
using System.Collections.Generic;
using System.Linq;
using Apic.Contracts.Infrastructure.Transfer.StatusResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Apic.Web.Extensions
{
	public static class ModelStateExtensions
	{
		public static List<ValidationErrorMessage> ToValidationErrorMessages(this ModelStateDictionary modelState)
		{
			return modelState.Keys
				.SelectMany(key =>
					modelState[key].Errors
						.Select(modelError => new ValidationErrorMessage(key, modelError.ErrorMessage)))
				.ToList();
		}
	}
}
