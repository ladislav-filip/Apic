using System;

namespace Apic.Contracts.Infrastructure.Transfer.StatusResults
{
    [Serializable]
    public class ValidationErrorMessage
	{
        public ValidationErrorMessage()
        {
        }

		public ValidationErrorMessage(string key, string message)
		{
			Key = key;
			Message = message;
		}

		public string Key { get; set; }
		public string Message { get; set; }
	}
}
