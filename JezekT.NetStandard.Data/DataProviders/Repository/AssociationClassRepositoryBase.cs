using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using JezekT.NetStandard.Data.EntityOperations;

namespace JezekT.NetStandard.Data.DataProviders.Repository
{
    public abstract class AssociationClassRepositoryBase<TEntity, FirstId, SecondId> 
        where TEntity : class, IAssociationClass<FirstId, SecondId>
    {
        private readonly IProvideAssociationClassItemByIds<TEntity, FirstId, SecondId> _entityProvider;
        private readonly ICreateEntity<TEntity> _entityCreator;
        private readonly IUpdateEntity<TEntity> _entityUpdater;
        private readonly IDeleteAssociationClassEntity<TEntity, FirstId, SecondId> _entityDestroyer;


        public async Task<TEntity> GetByIdsAsync(FirstId firstObjId, SecondId secondObjId)
        {
            return await _entityProvider.GetByIdsAsync(firstObjId, secondObjId);
        }

        public void Create(TEntity obj)
        {
            if (obj == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            _entityCreator.Create(obj);
        }

        public void Update(TEntity obj)
        {
            if (obj == null) throw new ArgumentNullException();
            Contract.EndContractBlock();
            
            _entityUpdater.Update(obj);
        }

        public void DeleteByIds(FirstId firstObjId, SecondId secondObjId)
        {
            _entityDestroyer.DeleteByIds(firstObjId, secondObjId);
        }


        protected AssociationClassRepositoryBase(IProvideAssociationClassItemByIds<TEntity, FirstId, SecondId> entityProvider, ICreateEntity<TEntity> entityCreator, 
            IUpdateEntity<TEntity> entityUpdater, IDeleteAssociationClassEntity<TEntity, FirstId, SecondId> entityDestroyer)
        {
            if (entityProvider == null || entityCreator == null || entityUpdater == null || entityDestroyer == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            _entityProvider = entityProvider;
            _entityCreator = entityCreator;
            _entityUpdater = entityUpdater;
            _entityDestroyer = entityDestroyer;
        }

    }
}
