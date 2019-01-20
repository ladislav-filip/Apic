using System;

namespace Apic.Common.Exceptions
{
    public class ObjectNotFoundException : Exception
    {
        public ObjectNotFoundException(string message = null) : base(message)
        {
        }
    }
}
