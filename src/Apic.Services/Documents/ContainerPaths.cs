using System;
using System.Collections.Generic;
using System.Text;

namespace Apic.Services.Documents
{
	public static class ContainerPaths
	{
		public static string CustomerDocuments(int customerId)
		{
			return "customer-" + customerId + "-documents";
		}
	}
}
