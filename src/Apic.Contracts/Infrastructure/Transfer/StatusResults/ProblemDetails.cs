using System;
using System.Net;

namespace Apic.Contracts.Infrastructure.Transfer.StatusResults
{
    [Serializable]
    public class ProblemDetails
	{
        public ProblemDetails()
        {
        }

		public string Type { get; set; }
		public string Title { get; set; }

		/// <summary>
		/// HTTP status code([RFC7231], Section 6)
		/// </summary>
		public int? Status { get; set; }
		public string Detail { get; set; }
		public string Instance { get; set; }

		public static ProblemDetails FromMessage(HttpStatusCode statusCode, string detail = null)
		{
			return new ProblemDetails()
			{
				Title = statusCode.ToString(),
				Detail = detail,
				Type = typeof(ProblemDetails).Name,
				Status = (int)statusCode
			};
		}

		public static ProblemDetails FromException(Exception ex, bool includeDetails)
		{
			var problemDetails =  new ProblemDetails()
			{
				Title = HttpStatusCode.InternalServerError.ToString(),
				Type = typeof(ProblemDetails).Name,
				Status = (int)HttpStatusCode.InternalServerError,
				Detail = ex.Message
            };

			if (includeDetails)
			{
				problemDetails.Detail += "\n" + ex.StackTrace;
			}

			return problemDetails;
		}
	}
}
