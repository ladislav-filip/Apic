using System.Collections.Generic;

namespace Apic.Contracts.Infrastructure.Transfer
{
	public class Result<T>
	{
		public T Data { get; set; }
        public List<string> Messages { get; set; }

  //      public static DataResult<T> Ok(T data)
		//{
		//	return new DataResult<T>()
		//	{
		//		Data = data,
		//		Code = ResultCodes.Ok
		//	};
		//}

		//public new static DataResult<T> ClientBadRequest(string message)
		//{
		//	var result =  new DataResult<T>()
		//	{
		//		Code = ResultCodes.BadRequest
		//	};

		//	result.AddMessage(message);

		//	return result;
		//}

		//public new static DataResult<T> Error(string message)
		//{
		//	var result = new DataResult<T>()
		//	{
		//		Code = ResultCodes.Error
		//	};

		//	result.AddMessage(message);

		//	return result;
		//}

		//public static DataResult<T> NotFound(string message = null)
		//{
		//	var result = new DataResult<T>()
		//	{
		//		Code = ResultCodes.NotFound
		//	};

		//	if (!string.IsNullOrEmpty(message))
		//	{
		//		result.AddMessage(message);
		//	}

		//	return result;
		//}
	}
}
