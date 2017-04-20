using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using JezekT.NetStandard.Data;
using JezekT.NetStandard.Services.DataProviders;
using JezekT.NetStandard.Services.Extensions;

namespace JezekT.NetStandard.Services.EntityOperations
{
    public abstract class AssociationClassCrudServiceBase<TEntity, FirstId, SecondId>
        where TEntity : class, IAssociationClass<FirstId, SecondId>
    {
        private readonly Dictionary<string, string> _validationErrors;

        private readonly IProvideAssociationClassItemByIds<TEntity, FirstId, SecondId> _entityProvider;
        private readonly ICreateEntityWithValidation<TEntity> _entityCreator;
        private readonly IUpdateEntityWithValidation<TEntity> _entityUpdater;
        private readonly IDeleteAssociationClassEntityWithValidation<TEntity, FirstId, SecondId> _entityDestroyer;


        public string ExceptionMessage { get; protected set; }
        public bool HasValidationError => _validationErrors.Any();

        public Dictionary<string, string> GetValidationErrors()
        {
            return _validationErrors;
        } 



        public async Task<TEntity> GetByIdsAsync(FirstId firstObjId, SecondId secondObjId)
        {
            return await _entityProvider.GetByIdsAsync(firstObjId, secondObjId);
        }

        public async Task<bool> CreateAsync(TEntity obj)
        {
            if (obj == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            var result = await _entityCreator.CreateAsync(obj);
            if (!result)
            {
                _entityCreator.ResolveErrors(_validationErrors, exception => ExceptionMessage = exception);
            }
            return result;
        }

        public async Task<bool> UpdateAsync(TEntity obj)
        {
            if (obj == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            var result = await _entityUpdater.UpdateAsync(obj);
            if (!result)
            {
                _entityUpdater.ResolveErrors(_validationErrors, exception => ExceptionMessage = exception);
            }
            return result;
        }

        public async Task<bool> DeleteByIdsAsync(FirstId firstObjId, SecondId secondObjId)
        {
            var result = await _entityDestroyer.DeleteByIdsAsync(firstObjId, secondObjId);
            if (!result)
            {
                _entityDestroyer.ResolveErrors(_validationErrors, exception => ExceptionMessage = exception);
            }
            return result;

        }


        protected AssociationClassCrudServiceBase(IProvideAssociationClassItemByIds<TEntity, FirstId, SecondId> entityProvider, ICreateEntityWithValidation<TEntity> entityCreator, 
            IUpdateEntityWithValidation<TEntity> entityUpdater, IDeleteAssociationClassEntityWithValidation<TEntity, FirstId, SecondId> entityDestroyer)
        {
            if (entityProvider == null || entityCreator == null || entityUpdater == null || entityDestroyer == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            _validationErrors = new Dictionary<string, string>();
            _entityProvider = entityProvider;
            _entityCreator = entityCreator;
            _entityUpdater = entityUpdater;
            _entityDestroyer = entityDestroyer;
        }

    }
}
