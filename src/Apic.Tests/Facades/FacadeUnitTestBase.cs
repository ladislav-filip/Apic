using Apic.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Apic.Tests.Facades
{
	public abstract class FacadeUnitTestBase
	{
		protected IUnitOfWork GetInMemoryDbContext(bool isolated)
		{
			DbContextOptionsBuilder<UnitOfWork> builder = new DbContextOptionsBuilder<UnitOfWork>()
				.UseInMemoryDatabase("default");

			IUnitOfWork dbContext = new UnitOfWork(builder.Options);

			if (isolated)
			{
				dbContext.Database.EnsureDeleted();
			}

			dbContext.Database.EnsureCreated();

			return dbContext;
		}

		private DbContextOptions<UnitOfWork> GetInMemoryOptions(string name = "Default")
		{
			return new DbContextOptionsBuilder<UnitOfWork>().UseInMemoryDatabase(name).Options;
		}
	}
}