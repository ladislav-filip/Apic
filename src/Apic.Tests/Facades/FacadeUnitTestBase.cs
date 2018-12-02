using Apic.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Apic.Tests.Facades
{
	public abstract class FacadeUnitTestBase
	{
		protected ApicDbContext GetInMemoryDbContext(bool isolated)
		{
			DbContextOptionsBuilder<ApicDbContext> builder = new DbContextOptionsBuilder<ApicDbContext>()
				.UseInMemoryDatabase("default");

			ApicDbContext dbContext = new ApicDbContext(builder.Options);

			if (isolated)
			{
				dbContext.Database.EnsureDeleted();
			}

			dbContext.Database.EnsureCreated();

			return dbContext;
		}

		private DbContextOptions<ApicDbContext> GetInMemoryOptions(string name = "Default")
		{
			return new DbContextOptionsBuilder<ApicDbContext>().UseInMemoryDatabase(name).Options;
		}
	}
}