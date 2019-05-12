using System;
using System.Collections.Generic;
using System.Text;
using Apic.Entities.Customers;

namespace Apic.Entities.Documents
{
	public class Document : IDomainEntity<Guid>
	{
		public Document()
		{
			Id = Guid.NewGuid();
		}

		public Guid Id { get; set; }
		public int CustomerId { get; set; }
		public string Name { get; set; }
		public string ContentType { get; set; }

		public Customer Customer { get; set; }
	}
}
