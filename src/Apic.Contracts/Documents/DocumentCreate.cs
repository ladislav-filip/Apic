using System;
using System.IO;

namespace Apic.Contracts.Documents
{
	public class DocumentCreate
	{
		public string Name { get; set; }
		public Stream Datastream { get; set; }
		public string ContentType { get; set; }
	}
}