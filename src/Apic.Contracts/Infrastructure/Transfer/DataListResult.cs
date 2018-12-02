using System.Collections.Generic;
using Apic.Contracts.Infrastructure.Transfer.Extensions;
using Apic.Contracts.Infrastructure.Transfer.Filters;

namespace Apic.Contracts.Infrastructure.Transfer
{
	public class DataListResult<T> : Result
	{
		public List<T> Data { get; set; }
		public int TotalItems { get; set; }
		public int PageSize { get; set; }
		public int Page { get; set; }

		public static DataListResult<T> Ok(List<T> data, PageFilter filter, int totalCount)
		{
			return new DataListResult<T>()
			{
				Data = data,
				Code = ResultCodes.Ok,
				Page = filter.Page,
				PageSize = filter.PageSize,
				TotalItems = totalCount
			};
		}

		public new static DataListResult<T> ClientBadRequest(string message)
		{
			var result = new DataListResult<T>()
			{
				Code = ResultCodes.BadRequest
			};

			result.AddMessage(message);

			return result;
		}

		public new static DataListResult<T> Error(string message)
		{
			var result = new DataListResult<T>()
			{
				Code = ResultCodes.Error
			};

			result.AddMessage(message);

			return result;
		}

		public static DataListResult<T> NotFound(string message = null)
		{
			var result = new DataListResult<T>()
			{
				Code = ResultCodes.NotFound
			};

			if (!string.IsNullOrEmpty(message))
			{
				result.AddMessage(message);
			}

			return result;
		}
	}
}