using System;
using System.Collections.Generic;
using System.Text;

namespace Apic.Contracts.Customers
{
	public class Customer
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
	}
}
