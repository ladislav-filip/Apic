using System.Collections.Generic;
using System.Net;

namespace Apic.Contracts.Infrastructure.Transfer.StatusResults
{
    public static class ValidationProblemDetailsHelper
    {
        public static ValidationProblemDetails FromErrors(List<ValidationErrorMessage> errors)
        {
            return new ValidationProblemDetails()
            {
                Status = (int)HttpStatusCode.BadRequest,
                Title = HttpStatusCode.BadRequest.ToString(),
                Type = typeof(ValidationProblemDetails).Name,
                Errors = errors
            };
        }
    }
}
