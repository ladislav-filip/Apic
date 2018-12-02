using System;
using System.Collections.Generic;
using System.Text;

namespace Apic.Contracts.Infrastructure.Transfer
{
	public enum ResultCodes
	{
		Ok = 200,
		Accepted = 202,
		BadRequest = 400,
		Unauthorized = 401,
		Forbidden = 403,
		NotFound = 404,
		NotAcceptable = 406,
		Error = 500
	}
}
