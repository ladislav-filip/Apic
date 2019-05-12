namespace Apic.Contracts.Customers
{
    public class Customer
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		
		/// <summary>
		/// Registration e-mail
		/// </summary>
		public string Email { get; set; }
	}
}
