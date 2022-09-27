using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JezekT.NetStandard.Data.DataProviders;
using Microsoft.EntityFrameworkCore;

namespace JezekT.NetStandard.Data.EntityFrameworkCore.DataProviders
{
    public class EntityProvider<TEntity, TId, TContext> : IProvideItemById<TEntity, TId>, IProvideItemByIdWithIncludes<TEntity, TId> 
        where TEntity : class, IWithId<TId>
        where TContext : DbContext
    {
        private readonly TContext _dbContext;
        

        public async Task<TEntity> GetByIdAsync(TId id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> GetByIdAsync(TId id, Expression<Func<TEntity, object>>[] includes)
        {
            var query = _dbContext.Set<TEntity>().AsQueryable();
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return await query.FirstOrDefaultAsync(x => Equals(x.Id, id));
        }


        public EntityProvider(TContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            _dbContext = dbContext;
        }
    }
}
