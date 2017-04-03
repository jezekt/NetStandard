using System;
using System.Diagnostics.Contracts;
using JezekT.NetStandard.Data.EntityOperations;
using Microsoft.EntityFrameworkCore;

namespace JezekT.NetStandard.Data.EntityFrameworkCore.EntityOperations
{
    public class EntityUpdater<TEntity, TContext> : IUpdateEntity<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        private readonly TContext _dbContext;


        public void Update(TEntity obj)
        {
            if (obj == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            _dbContext.Attach(obj);
            var entry = _dbContext.Entry(obj);
            entry.State = EntityState.Modified;
        }


        public EntityUpdater(TContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            _dbContext = dbContext;
        }

    }
}
