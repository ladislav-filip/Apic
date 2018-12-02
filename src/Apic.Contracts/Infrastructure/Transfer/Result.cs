using System.Collections.Generic;

namespace Apic.Contracts.Infrastructure.Transfer
{
	public class Result : IResult
	{
		public Result()
		{
			Messages = new List<ResultMessage>();
		}

		public ResultCodes Code { get; set; } = ResultCodes.Ok;
		public List<ResultMessage> Messages { get; set; }

		public static Result Ok()
		{
			return new Result();
		}

		public static Result ClientBadRequest(string message)
		{
			return new Result()
			{
				Code = ResultCodes.BadRequest

			};
		}

		public static Result Error(string message)
		{
			return new Result()
			{
				Code = ResultCodes.Error
			};
		}

		public static Result NotFound()
		{
			return new Result()
			{
				Code = ResultCodes.NotFound
			};
		}
	}
}
