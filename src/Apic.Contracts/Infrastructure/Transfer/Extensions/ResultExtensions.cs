namespace Apic.Contracts.Infrastructure.Transfer.Extensions
{
	public static class ResultExtensions
	{
		public static Result AddMessage(this Result result, string message, string propertyName = null)
		{
			result.Messages.Add(new ResultMessage(message, propertyName));

			return result;
		}
	}
}