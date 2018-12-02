namespace Apic.Contracts.Infrastructure.Transfer
{
	public class ResultMessage
	{
		public ResultMessage(string message, string property)
		{
			Message = message;
			Property = property;
		}

		public string Message { get; set; }
		public string Property { get; set; }
	}
}