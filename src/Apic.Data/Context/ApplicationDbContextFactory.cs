using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Apic.Data.Context
{
	public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApicDbContext>
	{
		public ApicDbContext CreateDbContext(string[] args)
		{
			var builder = new DbContextOptionsBuilder<ApicDbContext>();
			builder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=apic;User Id=sa;Password=asdfghjkl;Integrated Security=false;Application Name=Apic");

			return new ApicDbContext(builder.Options);
		}
	}
}
