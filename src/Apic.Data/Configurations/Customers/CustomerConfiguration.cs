using Apic.Entities.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apic.Data.Configurations.Customers
{
	public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
	{
		public void Configure(EntityTypeBuilder<Customer> builder)
		{
			builder.ToTable("Customers");

			builder.HasKey(x => x.Id);
			builder.Property(x => x.Id).ValueGeneratedOnAdd();

			builder.Property(x => x.Email).IsRequired();

			builder.OwnsOne(x => x.BillingAddress);
			builder.OwnsOne(x => x.DeliveryAddress);

			builder.HasMany(x => x.Orders).WithOne(x => x.Customer).HasForeignKey(x => x.CustomerId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasMany(x => x.Documents).WithOne(x => x.Customer).HasForeignKey(x => x.CustomerId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
