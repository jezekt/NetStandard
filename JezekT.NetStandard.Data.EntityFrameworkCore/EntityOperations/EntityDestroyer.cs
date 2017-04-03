using System;
using System.Diagnostics.Contracts;
using JezekT.NetStandard.Data.EntityOperations;
using Microsoft.EntityFrameworkCore;

namespace JezekT.NetStandard.Data.EntityFrameworkCore.EntityOperations
{
    public class EntityDestroyer<TEntity, TId, TContext> : IDeleteEntity<TEntity, TId>
        where TEntity : class
        where TContext : DbContext
    {
        private readonly TContext _dbContext;


        public virtual void DeleteById(TId id)
        {
            var objToRemove = _dbContext.Set<TEntity>().Find(id);
            if (objToRemove == null) throw new InvalidOperationException();
            _dbContext.Set<TEntity>().Remove(objToRemove);
        }


        public EntityDestroyer(TContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            _dbContext = dbContext;
        }
    }
}
