using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Apic.Data.Context
{
	public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<UnitOfWork>
	{
		public UnitOfWork CreateDbContext(string[] args)
		{
			var builder = new DbContextOptionsBuilder<UnitOfWork>();
			builder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=apic;User Id=sa;Password=asdfghjkl;Integrated Security=false;Application Name=Apic");

			return new UnitOfWork(builder.Options);
		}
	}
}
