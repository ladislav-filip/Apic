using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apic.Data.Configurations.Customers;
using Apic.Data.Configurations.Documents;
using Apic.Data.Configurations.Orders;
using Apic.Data.Infrastructure;
using Apic.Entities.Customers;
using Apic.Entities.Documents;
using Microsoft.EntityFrameworkCore;

namespace Apic.Data.Context
{
	public class ApicDbContext : DbContext
	{
		public ApicDbContext(DbContextOptions<ApicDbContext> options) : base(options)
		{
		}

		public DbSet<Customer> Customers { get; set; }
		public DbSet<Document> Documents { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new CustomerConfiguration());
			modelBuilder.ApplyConfiguration(new DocumentConfiguration());
			modelBuilder.ApplyConfiguration(new OrderConfiguration());

			base.OnModelCreating(modelBuilder);
		}
	}
}
