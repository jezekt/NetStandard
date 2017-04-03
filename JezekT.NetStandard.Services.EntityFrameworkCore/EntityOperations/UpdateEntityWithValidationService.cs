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
    public class UpdateEntityWithValidationService<TEntity> : ValidationBase<TEntity>, IUpdateEntityWithValidation<TEntity>
        where TEntity : class 
    {
        private readonly IUpdateEntity<TEntity> _repository;
        private readonly IUnitOfWork _unitOfWork;


        public async Task<bool> UpdateAsync(TEntity obj)
        {
            if (obj == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            try
            {
                if (Validation == null || Validation.Validate(obj))
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


        public UpdateEntityWithValidationService(IUpdateEntity<TEntity> repository, IUnitOfWork unitOfWork, IValidation<TEntity> validation = null)
            : base(validation)
        {
            if (repository == null || unitOfWork == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            _repository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
