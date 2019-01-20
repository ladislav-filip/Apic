using System;

namespace Apic.Common.Exceptions
{
    public class RequestFailedException : Exception
    {
        public RequestFailedException(string message = null) : base(message)
        {
        }
    }
}
