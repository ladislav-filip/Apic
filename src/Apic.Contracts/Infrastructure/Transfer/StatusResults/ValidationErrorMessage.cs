namespace Apic.Contracts.Infrastructure.Transfer.StatusResults
{
	public class ValidationErrorMessage
	{
		public ValidationErrorMessage(string key, string message)
		{
			Key = key;
			Message = message;
		}

		public string Key { get; set; }
		public string Message { get; set; }
	}
}