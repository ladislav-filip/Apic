using System;
using System.Collections.Generic;
using Apic.Entities.Customers;

namespace Apic.Entities.Orders
{
	public class Order : IDomainEntity<Guid>
	{
		public Guid Id { get; set; }
		public int CustomerId { get; set; }
		public string Name { get; set; }

		public Customer Customer { get; set; }
		public  List<OrderItem> OrderItems { get; set; }
	}
}
