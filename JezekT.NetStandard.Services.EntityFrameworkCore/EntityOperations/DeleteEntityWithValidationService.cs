using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using JezekT.NetStandard.Data.DataProviders.Repository;
using JezekT.NetStandard.Data.EntityOperations;
using JezekT.NetStandard.Services.EntityFrameworkCore.Resources;
using JezekT.NetStandard.Services.EntityOperations;
using JezekT.NetStandard.Validation;
using Microsoft.EntityFrameworkCore;

namespace JezekT.NetStandard.Services.EntityFrameworkCore.EntityOperations
{
    public class DeleteEntityWithValidationService<TEntity, TId> : ValidationBase<TEntity>, IDeleteEntityWithValidation<TEntity, TId>
        where TEntity : class
    {
        private readonly IDeleteEntity<TEntity, TId> _repository;
        private readonly IUnitOfWork _unitOfWork;


        public async Task<bool> DeleteByIdAsync(TId id)
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


        public DeleteEntityWithValidationService(IDeleteEntity<TEntity, TId> repository, IUnitOfWork unitOfWork, IValidation<TEntity> validation = null)
            : base(validation)
        {
            if (repository == null || unitOfWork == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            _repository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
