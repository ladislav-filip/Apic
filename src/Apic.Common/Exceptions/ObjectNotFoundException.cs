using System;
using System.Net;

namespace Apic.Common.Exceptions
{
    public class ObjectNotFoundException : Exception, IApiException
    {
        public ObjectNotFoundException(string message = null) : base(message)
        {
        }

        public string Title { get; set; } = "Object not found";
        public int StatusCode { get; set; } = (int)HttpStatusCode.NotFound;
    }
}
