using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using JezekT.NetStandard.Data.EntityOperations;
using Microsoft.EntityFrameworkCore;

namespace JezekT.NetStandard.Data.EntityFrameworkCore.EntityOperations
{
    public class AssociationClassEntityDestroyer<TEntity, FirstId, SecondId, TContext> : IDeleteAssociationClassEntity<TEntity, FirstId, SecondId>
        where TEntity : class, IAssociationClass<FirstId, SecondId>
        where TContext : DbContext
    {
        private readonly TContext _dbContext;


        public void DeleteByIds(FirstId firstObjId, SecondId secondObjId)
        {
            var objToRemove = _dbContext.Set<TEntity>().FirstOrDefault(x => EqualityComparer<FirstId>.Default.Equals(x.FirstObjId, firstObjId) &&
                                                                            EqualityComparer<SecondId>.Default.Equals(x.SecondObjId, secondObjId));
            if (objToRemove == null) throw new InvalidOperationException();
            _dbContext.Set<TEntity>().Remove(objToRemove);
        }


        public AssociationClassEntityDestroyer(TContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            _dbContext = dbContext;
        }
    }
}
