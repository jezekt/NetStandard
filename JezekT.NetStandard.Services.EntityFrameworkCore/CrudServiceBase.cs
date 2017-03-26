using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using JezekT.NetStandard.Data;
using JezekT.NetStandard.Data.Repository;
using JezekT.NetStandard.Services.EntityFrameworkCore.Resources;
using JezekT.NetStandard.Validation;
using Microsoft.EntityFrameworkCore;

namespace JezekT.NetStandard.Services.EntityFrameworkCore
{
    public abstract class CrudServiceBase<TEntity, TId>
        where TEntity : class, IWithId<TId>
    {
        private readonly IValidation<TEntity> _validation;
        private readonly IRepository<TEntity> _repository;
        private readonly IUnitOfWork _unitOfWork;


        public string ExceptionMessage { get; protected set; }
        public bool HasValidationError
        {
            get
            {
                if (_validation == null) return false;
                var errors = _validation.GetValidationErrors();
                return errors != null && errors.Any();
            }
        }
        public Dictionary<string, string> GetValidationErrors()
        {
            return _validation?.GetValidationErrors();
        }


        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> CreateAsync(TEntity obj)
        {
            if (obj == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            try
            {
                if (_validation == null || _validation.Validate(obj))
                {
                    _repository.Create(obj);
                    await _unitOfWork.SaveChangesAsync();
                    return true;
                }
            }
            catch (DbUpdateException)
            {
                ExceptionMessage = ResourcesSettings.CreateErrorMessage;
            }
            return false;
        }

        public async Task<bool> UpdateAsync(TEntity obj)
        {
            if (obj == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            try
            {
                if (_validation == null || _validation.Validate(obj))
                {
                    _repository.Update(obj);
                    await _unitOfWork.SaveChangesAsync();
                    return true;
                }
            }
            catch (DbUpdateException)
            {
                ExceptionMessage = ResourcesSettings.EditErrorMessage;
            }
            return false;
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            try
            {
                _repository.DeleteById(id);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                ExceptionMessage = ResourcesSettings.DeleteErrorMessage;
            }
            catch (InvalidOperationException)
            {
                ExceptionMessage = ResourcesSettings.InvalidOperationMessage;
            }
            return false;
        }


        protected CrudServiceBase(IRepository<TEntity> repository, IUnitOfWork unityOfWork, IValidation<TEntity> validation = null)
        {
            if (repository == null || unityOfWork == null ) throw new ArgumentNullException();
            Contract.EndContractBlock();

            _validation = validation;
            _repository = repository;
            _unitOfWork = unityOfWork;
        }
    }
}
