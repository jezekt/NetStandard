﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JezekT.NetStandard.Data;
using JezekT.NetStandard.Services.DataProviders;
using JezekT.NetStandard.Services.Extensions;

namespace JezekT.NetStandard.Services.EntityOperations
{
    public abstract class CrudWithValidationServiceBase<TEntity, TId>
        where TEntity : class, IWithId<TId>
    {
        private readonly Dictionary<string, string> _validationErrors;

        private readonly IProvideItemById<TEntity, TId> _entityProvider;
        private readonly IProvideItemByIdWithIncludes<TEntity, TId> _entityWithIncludesProvider;
        private readonly ICreateEntityWithValidation<TEntity> _entityCreator;
        private readonly IUpdateEntityWithValidation<TEntity> _entityUpdater;
        private readonly IDeleteEntityWithValidation<TEntity, TId> _entityDestroyer;

        public string ExceptionMessage { get; protected set; }
        public bool HasValidationError => _validationErrors.Any();

        public Dictionary<string, string> GetValidationErrors()
        {
            return _validationErrors;
        }


        public async Task<TEntity> GetByIdAsync(TId id)
        {
            if (_entityProvider == null)
            {
                throw new NullReferenceException("Entity provider is null. Make sure it is injected in constructor.");
            }
            return await _entityProvider.GetByIdAsync(id);
        }

        public async Task<TEntity> GetByIdAsync(TId id, params Expression<Func<TEntity, object>>[] includes)
        {
            if (_entityWithIncludesProvider == null)
            {
                throw new NullReferenceException("Entity provider with includes is null. Make sure it is injected in constructor.");
            }
            return await _entityWithIncludesProvider.GetByIdAsync(id, includes);
        }


        public virtual async Task<bool> CreateAsync(TEntity obj)
        {
            if (obj == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            if (_entityWithIncludesProvider == null)
            {
                throw new NullReferenceException("Entity creator is null. Make sure it is injected in constructor.");
            }
            var result = await _entityCreator.CreateAsync(obj);
            if (!result)
            {
                _entityCreator.ResolveErrors(_validationErrors, exception => ExceptionMessage = exception);    
            }
            return result;
        }

        public virtual async Task<bool> UpdateAsync(TEntity obj)
        {
            if (obj == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            if (_entityWithIncludesProvider == null)
            {
                throw new NullReferenceException("Entity updater is null. Make sure it is injected in constructor.");
            }
            var result = await _entityUpdater.UpdateAsync(obj);
            if (!result)
            {
                _entityUpdater.ResolveErrors(_validationErrors, exception => ExceptionMessage = exception);
            }
            return result;
        }

        public virtual async Task<bool> DeleteByIdAsync(TId id)
        {
            if (_entityWithIncludesProvider == null)
            {
                throw new NullReferenceException("Entity destroyer is null. Make sure it is injected in constructor.");
            }
            var result = await _entityDestroyer.DeleteByIdAsync(id);
            if (!result)
            {
                _entityDestroyer.ResolveErrors(_validationErrors, exception => ExceptionMessage = exception);
            }
            return result;
        }


        protected CrudWithValidationServiceBase(IProvideItemById<TEntity, TId> entityProvider = null, IProvideItemByIdWithIncludes<TEntity, TId> entityWithIncludesProvider = null,
            ICreateEntityWithValidation<TEntity> entityCreator = null, IUpdateEntityWithValidation<TEntity> entityUpdater = null, IDeleteEntityWithValidation<TEntity, TId> entityDestroyer = null)
        {
            _validationErrors = new Dictionary<string, string>();
            _entityProvider = entityProvider;
            _entityWithIncludesProvider = entityWithIncludesProvider;
            _entityCreator = entityCreator;
            _entityUpdater = entityUpdater;
            _entityDestroyer = entityDestroyer;
        }
    }
}
