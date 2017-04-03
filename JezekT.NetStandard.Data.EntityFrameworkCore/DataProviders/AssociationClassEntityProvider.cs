using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using JezekT.NetStandard.Data.DataProviders;
using Microsoft.EntityFrameworkCore;

namespace JezekT.NetStandard.Data.EntityFrameworkCore.DataProviders
{
    public class AssociationClassEntityProvider<TEntity, FirstId, SecondId, TContext> : IProvideAssociationClassItemByIds<TEntity, FirstId, SecondId>
        where TEntity : class, IAssociationClass<FirstId, SecondId>
        where TContext : DbContext
    {
        private readonly TContext _dbContext;


        public async Task<TEntity> GetByIdsAsync(FirstId firstObjId, SecondId secondObjId)
        {
            return await _dbContext.Set<TEntity>().Where(x => EqualityComparer<FirstId>.Default.Equals(x.FirstObjId, firstObjId) &&
                                                              EqualityComparer<SecondId>.Default.Equals(x.SecondObjId, secondObjId))
                                                  .FirstOrDefaultAsync();
        }


        public AssociationClassEntityProvider(TContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            _dbContext = dbContext;
        }
    }
}
