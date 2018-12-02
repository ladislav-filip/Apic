using System;
using System.Collections.Generic;
using System.Text;
using Apic.Entities.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apic.Data.Configurations.Documents
{
	public class DocumentConfiguration : IEntityTypeConfiguration<Document>
	{
		public void Configure(EntityTypeBuilder<Document> builder)
		{
			builder.ToTable("Documents");

			builder.HasKey(x => x.Id);
			builder.Property(x => x.Id).ValueGeneratedNever();

			builder.HasOne(x => x.Customer).WithMany(x => x.Documents).HasForeignKey(x => x.CustomerId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
