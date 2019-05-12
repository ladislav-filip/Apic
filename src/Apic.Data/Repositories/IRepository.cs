using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Apic.Data.Repositories
{
    public interface IRepository<TEntity, TKey> where TEntity : class
    {
        TEntity Find(TKey id);
        List<TEntity> GetAll();
        TEntity FindOrDefault(TKey id);
        List<TEntity> Query(Expression<Func<TEntity, bool>> predicate);
    }
}