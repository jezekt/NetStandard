using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JezekT.NetStandard.Data.EntityOperations;

namespace JezekT.NetStandard.Data.DataProviders.Repository
{
    public abstract class RepositoryBase<TEntity, TId> 
        where TEntity : class, IWithId<TId>
    {
        private readonly IProvideItemById<TEntity, TId> _entityByIdProvider;
        private readonly IProvideItemByIdWithIncludes<TEntity, TId> _entityWithIncludesByIdProvider;
        private readonly ICreateEntity<TEntity> _entityCreator;
        private readonly IUpdateEntity<TEntity> _entityUpdater;
        private readonly IDeleteEntity<TEntity, TId> _entityDestroyer;


        public virtual async Task<TEntity> GetByIdAsync(TId id)
        {
            return await _entityByIdProvider.GetByIdAsync(id);
        }

        public virtual async Task<TEntity> GetByIdAsync(TId id, Expression<Func<TEntity, object>>[] includes)
        {
            return await _entityWithIncludesByIdProvider.GetByIdAsync(id, includes);
        }


        public virtual void Create(TEntity obj)
        {
            _entityCreator.Create(obj);
        }

        public virtual void Update(TEntity obj)
        {
            _entityUpdater.Update(obj);
        }

        public virtual void DeleteById(TId id)
        {
            _entityDestroyer.DeleteById(id);
        }


        protected RepositoryBase(IProvideItemById<TEntity, TId> entityByIdProvider, IProvideItemByIdWithIncludes<TEntity, TId> entityWithIncludesByIdProvider,
            ICreateEntity<TEntity> entityCreator, IUpdateEntity<TEntity> entityUpdater, IDeleteEntity<TEntity, TId> entityDestroyer)
        {
            if (entityByIdProvider == null || entityWithIncludesByIdProvider == null || entityCreator == null ||
                entityUpdater == null || entityDestroyer == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            _entityByIdProvider = entityByIdProvider;
            _entityWithIncludesByIdProvider = entityWithIncludesByIdProvider;
            _entityCreator = entityCreator;
            _entityUpdater = entityUpdater;
            _entityDestroyer = entityDestroyer;
        }

    }
}
