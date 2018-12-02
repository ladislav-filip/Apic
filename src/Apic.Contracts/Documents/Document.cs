using System;

namespace Apic.Contracts.Documents
{
	public class Document
	{
		public Guid Id { get; set; }
		public int CustomerId { get; set; }
		public string Url { get; set; }
		public string Name { get; set; }
		public string Availability { get; set; }
		public string ContentType { get; set; }
	}
}
