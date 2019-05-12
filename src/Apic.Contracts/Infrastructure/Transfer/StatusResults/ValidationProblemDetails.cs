using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace Apic.Contracts.Infrastructure.Transfer.StatusResults
{
    [Serializable]
    public class ValidationProblemDetails : ProblemDetails
    {
        public ValidationProblemDetails()
        {
            Errors = new List<ValidationErrorMessage>();
        }

        public List<ValidationErrorMessage> Errors { get; set; }

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
