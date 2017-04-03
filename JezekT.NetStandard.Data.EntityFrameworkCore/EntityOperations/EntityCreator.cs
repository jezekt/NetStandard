using System;
using System.Diagnostics.Contracts;
using JezekT.NetStandard.Data.EntityOperations;
using Microsoft.EntityFrameworkCore;

namespace JezekT.NetStandard.Data.EntityFrameworkCore.EntityOperations
{
    public class EntityCreator<TEntity, TContext> : ICreateEntity<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        private readonly TContext _dbContext;


        public void Create(TEntity obj)
        {
            if (obj == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            _dbContext.Set<TEntity>().Add(obj);
        }


        public EntityCreator(TContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            _dbContext = dbContext;
        }
    }
}
