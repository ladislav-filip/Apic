using System.Linq;

namespace Apic.Facades.Infrastructure
{
    public interface IQuery<T>
    {
        IQueryable<T> Query();
        int Count();
    }
}
