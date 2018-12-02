using System.Linq;
using Apic.Data.Context;

namespace Apic.Data.Infrastructure
{
	public interface IQuery<T>
	{
		IQueryable<T> Execute(IQueryable<T> db);
	}
}