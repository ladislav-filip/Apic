﻿using System.Net;
using System.Net.Http;
using Apic.Services;
using Apic.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ValidationProblemDetails = Apic.Contracts.Infrastructure.Transfer.StatusResults.ValidationProblemDetails;

namespace Apic.Web.Filters.Action
{
    /// <summary>
    /// Zpřístupní ModelState skrze ModelState accessor
    /// Pokud není ModelState validní, ukončuje zpracování
    /// </summary>
    public class ValidationFilter : IActionFilter
    {
        private readonly ModelStateAccessor modelStateAccessor;

        public ValidationFilter(ModelStateAccessor modelStateAccessor)
        {
            this.modelStateAccessor = modelStateAccessor;
        }

        public void OnActionExecuting(ActionExecutingContext context)
		{
            modelStateAccessor.ModelState = context.ModelState;
		}

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var message = ValidationProblemDetails.FromErrors(context.ModelState.ToValidationErrorMessages());
                context.Result = new BadRequestObjectResult(message);
            }
        }
    }
}
