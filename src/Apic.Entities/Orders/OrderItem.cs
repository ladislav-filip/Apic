using System;

namespace Apic.Entities.Orders
{
	public class OrderItem
	{
		public Guid Id { get; set; }
		public Guid OrderId { get; set; }
		public string Title { get; set; }

		public Order Order { get; set; }
	}
}
