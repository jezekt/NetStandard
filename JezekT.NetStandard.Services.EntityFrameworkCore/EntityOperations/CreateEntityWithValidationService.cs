using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using JezekT.NetStandard.Data.DataProviders.Repository;
using JezekT.NetStandard.Data.EntityOperations;
using JezekT.NetStandard.Services.EntityFrameworkCore.Resources;
using JezekT.NetStandard.Services.EntityOperations;
using JezekT.NetStandard.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JezekT.NetStandard.Services.EntityFrameworkCore.EntityOperations
{
    public class CreateEntityWithValidationService<TEntity> : ValidationBase<TEntity>, ICreateEntityWithValidation<TEntity>
        where TEntity : class
    {
        private readonly ICreateEntity<TEntity> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateEntityWithValidationService<TEntity>> _logger;

        
        public async Task<bool> CreateAsync(TEntity obj)
        {
            if (obj == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            try
            {
                if (Validation == null || Validation.Validate(obj))
                {
                    _repository.Create(obj);
                    await _unitOfWork.SaveChangesAsync();
                    return true;
                }
            }
            catch (DbUpdateException ex)
            {
                ExceptionMessage = ResourcesSettings.CreateErrorMessage;
                _logger?.LogError(ex.Message);
            }
            return false;
        }


        public CreateEntityWithValidationService(ICreateEntity<TEntity> repository, IUnitOfWork unitOfWork, IValidation<TEntity> validation = null, 
            ILogger<CreateEntityWithValidationService<TEntity>> logger = null)
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
