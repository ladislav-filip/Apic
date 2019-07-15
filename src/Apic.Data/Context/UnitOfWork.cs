using Apic.Data.Configurations;
using Apic.Data.Configurations.Customers;
using Apic.Data.Configurations.Documents;
using Apic.Data.Configurations.Orders;
using Apic.Data.Repositories;
using Apic.Entities;
using Apic.Entities.Customers;
using Apic.Entities.Documents;
using Microsoft.EntityFrameworkCore;

namespace Apic.Data.Context
{
    public sealed class UnitOfWork : DbContext, IUnitOfWork
	{
		private CustomerRepository customerRepository;
		private DocumentRepository documentRepository;

		public CustomerRepository Customers => customerRepository ?? (customerRepository = new CustomerRepository(Set<Customer>()));
		public DocumentRepository Documents => documentRepository ?? (documentRepository = new DocumentRepository(Set<Document>()));
		
		public UnitOfWork(DbContextOptions<UnitOfWork> options) : base(options)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.EnableSensitiveDataLogging(true);
			optionsBuilder.EnableDetailedErrors(true);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(EfCoreEntitiesFlag).Assembly);

			base.OnModelCreating(modelBuilder);
		}
	}
}
