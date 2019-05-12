using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace Apic.Contracts.Infrastructure.Transfer.StatusResults
{
    public static class ValidationProblemDetailsHelper
    {
        public static ValidationProblemDetails FromErrors(List<ValidationErrorMessage> errors)
        {
            return new ValidationProblemDetails()
            {
                Status = StatusCodes.Status400BadRequest,
                Title = HttpStatusCode.BadRequest.ToString(),
                Type = typeof(ValidationProblemDetails).Name,
                Errors = errors
            };
        }
    }
}
