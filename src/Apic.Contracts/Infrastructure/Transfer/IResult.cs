using System.Collections.Generic;

namespace Apic.Contracts.Infrastructure.Transfer
{
	public interface IResult
	{
		ResultCodes Code { get; set; }
		List<ResultMessage> Messages { get; set; }
	}
}