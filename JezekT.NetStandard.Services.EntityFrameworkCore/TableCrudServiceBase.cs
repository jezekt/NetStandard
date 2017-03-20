using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JezekT.NetStandard.Data;
using JezekT.NetStandard.Data.Repository;
using JezekT.NetStandard.Pagination;
using JezekT.NetStandard.Pagination.DataProviders;
using JezekT.NetStandard.Services.EntityFrameworkCore.Resources;
using JezekT.NetStandard.Validation;
using Microsoft.EntityFrameworkCore;

namespace JezekT.NetStandard.Services.EntityFrameworkCore
{
    public abstract class TableCrudServiceBase<T, TEntity, TId> 
        where T : class
        where TEntity : class, IWithId<TId>
    {
        private readonly IValidation<T> _validation;
        private readonly IRepository<TEntity> _repository;
        private readonly IPaginationDataProvider<TEntity, TId> _paginationDataProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        
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

        
        public async Task<T> GetByIdAsync(int id)
        {
            var objDb = await _repository.GetByIdAsync(id);
            if (objDb == null)
            {
                return null;
            }
            return _mapper.Map<TEntity, T>(objDb);
        }

        public async Task<bool> CreateAsync(T obj)
        {
            if (obj == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            var objDb = _mapper.Map<T, TEntity>(obj);
            try
            {
                if (_validation == null || _validation.Validate(obj))
                {
                    _repository.Create(objDb);
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

        public async Task<bool> UpdateAsync(T obj)
        {
            if (obj == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            var objDb = _mapper.Map<T, TEntity>(obj);
            try
            {
                if (_validation == null || _validation.Validate(obj))
                {
                    _repository.Update(objDb);
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


        public async Task<IPaginationData> GetPaginationDataAsync(string term, int start, int pageSize, string orderField, string orderDirection, TId[] inputFiletrIds = null)
        {
            return await _paginationDataProvider.GetPaginationDataAsync(term, start, pageSize, orderField, orderDirection, inputFiletrIds);
        }
        

        protected TableCrudServiceBase(IRepository<TEntity> repository, IPaginationDataProvider<TEntity, TId> paginationDataProvider, 
            IUnitOfWork unityOfWork, IMapper mapper, IValidation<T> validation = null)
        {
            if (repository == null || paginationDataProvider == null || unityOfWork == null || mapper == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            _validation = validation;
            _repository = repository;
            _paginationDataProvider = paginationDataProvider;
            _unitOfWork = unityOfWork;
            _mapper = mapper;
        }
    }
}
