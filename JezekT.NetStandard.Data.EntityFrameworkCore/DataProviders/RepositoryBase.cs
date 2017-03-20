using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace JezekT.NetStandard.Data.EntityFrameworkCore.DataProviders
{
    public abstract class RepositoryBase<TEntity, TId, TContext> 
        where TEntity : class, IWithId<TId>
        where TContext : DbContext
    {
        protected TContext DbContext { get; }


        public virtual async Task<TEntity> GetByIdAsync(TId id)
        {
            return await DbContext.Set<TEntity>().FindAsync(id);
        }
        
        public virtual void Create(TEntity obj)
        {
            if (obj == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            DbContext.Set<TEntity>().Add(obj);
        }

        public virtual void Update(TEntity obj)
        {
            if (obj == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            DbContext.Attach(obj);
            var entry = DbContext.Entry(obj);
            entry.State = EntityState.Modified;
        }

        public virtual void DeleteById(TId id)
        {
            var objToRemove = DbContext.Set<TEntity>().Find(id);
            if (objToRemove == null) throw new InvalidOperationException();
            DbContext.Set<TEntity>().Remove(objToRemove);
        }


        protected RepositoryBase(TContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            DbContext = dbContext;
        }

    }
}
