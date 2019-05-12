using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Apic.Entities;
using Microsoft.EntityFrameworkCore;

namespace Apic.Data.Repositories
{
    public class BaseRepository<TEntity, TKey>: IRepository<TEntity, TKey> where TEntity: class, IDomainEntity<TKey>
    {
        protected readonly DbSet<TEntity> DbSet; 
        
        public BaseRepository(DbSet<TEntity> dbSet)
        {
            DbSet = dbSet;
        }
        
        public virtual TEntity Find(TKey id)
        {
            return DbSet.Find(id);
        }

        public virtual List<TEntity> GetAll()
        {
            return DbSet.AsQueryable().ToList();
        }

        public virtual TEntity FindOrDefault(TKey id) 
        {
            return DbSet.Find(id);
        }

        public virtual List<TEntity> Query(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.AsQueryable().Where(predicate).ToList();
        }
    }
}