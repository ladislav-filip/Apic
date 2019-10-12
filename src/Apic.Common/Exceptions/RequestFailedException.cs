using System;
using System.Net;

namespace Apic.Common.Exceptions
{
    public class RequestFailedException : Exception, IApiException
    {
        public RequestFailedException(string message = null) : base(message)
        {
        }
        
        public string Title { get; set; } = "Request cannot be processed";
        public int StatusCode { get; set; } = (int)HttpStatusCode.BadRequest;
    }
}
