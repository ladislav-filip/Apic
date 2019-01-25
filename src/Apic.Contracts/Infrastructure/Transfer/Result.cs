using System.Collections.Generic;

namespace Apic.Contracts.Infrastructure.Transfer
{
	public class Result<T>
	{
		public T Data { get; set; }
        public List<string> Messages { get; set; }
	}
}
