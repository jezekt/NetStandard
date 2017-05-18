using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using JezekT.NetStandard.Data.DataProviders.Repository;
using JezekT.NetStandard.Data.EntityOperations;
using JezekT.NetStandard.Services.EntityOperations;
using JezekT.NetStandard.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JezekT.NetStandard.Services.EntityFrameworkCore.EntityOperations
{
    public class DeleteEntityWithValidationService<TEntity, TId> : ValidationServiceBase<TEntity>, IDeleteEntityWithValidation<TEntity, TId>
        where TEntity : class
    {
        private readonly IDeleteEntity<TEntity, TId> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteEntityWithValidationService<TEntity, TId>> _logger;


        public async Task<bool> DeleteByIdAsync(TId id)
        {
            try
            {
                _repository.DeleteById(id);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                ExceptionMessage = Properties.Resources.DeleteErrorMessage;
                _logger?.LogError(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                ExceptionMessage = Properties.Resources.InvalidOperationMessage;
                _logger?.LogError(ex.Message);
            }
            return false;
        }


        public DeleteEntityWithValidationService(IDeleteEntity<TEntity, TId> repository, IUnitOfWork unitOfWork, IValidation<TEntity> validation = null, 
            ILogger<DeleteEntityWithValidationService<TEntity, TId>> logger = null)
            : base(validation)
        {
            if (repository == null || unitOfWork == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            _repository = repository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
    }
}
