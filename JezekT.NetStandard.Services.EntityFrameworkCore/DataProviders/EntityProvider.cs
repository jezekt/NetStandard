using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JezekT.NetStandard.Services.DataProviders;

namespace JezekT.NetStandard.Services.EntityFrameworkCore.DataProviders
{
    public class EntityProvider<TEntity, TId> : IProvideItemById<TEntity, TId>, IProvideItemByIdWithIncludes<TEntity, TId> 
        where TEntity : class
    {
        private readonly Data.DataProviders.IProvideItemById<TEntity, TId> _entityProvider;
        private readonly Data.DataProviders.IProvideItemByIdWithIncludes<TEntity, TId> _entityWithIncludesProvider;


        public async Task<TEntity> GetByIdAsync(TId id)
        {
            return await _entityProvider.GetByIdAsync(id);
        }

        public async Task<TEntity> GetByIdAsync(TId id, params Expression<Func<TEntity, object>>[] includes)
        {
            return await _entityWithIncludesProvider.GetByIdAsync(id, includes);
        }


        public EntityProvider(Data.DataProviders.IProvideItemById<TEntity, TId> entityProvider,
            Data.DataProviders.IProvideItemByIdWithIncludes<TEntity, TId> entityWithIncludesProvider)
        {
            if (entityProvider == null || entityWithIncludesProvider == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            _entityProvider = entityProvider;
            _entityWithIncludesProvider = entityWithIncludesProvider;
        }
    }
}
