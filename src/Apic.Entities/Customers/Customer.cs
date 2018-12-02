using System;
using System.Collections.Generic;
using System.Text;
using Apic.Entities.Documents;
using Apic.Entities.Orders;
using Apic.Entities.OwnedTypes;

namespace Apic.Entities.Customers
{
	public class Customer
	{
		public Customer()
		{
			BillingAddress = new Address();
			DeliveryAddress = new Address();
			Orders = new List<Order>();
			Documents = new List<Document>();

		}
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }

		public Address BillingAddress { get; set; }
		public Address DeliveryAddress { get; set; }

		public List<Order> Orders { get; set; }
		public List<Document> Documents { get; set; }
	}
}
