﻿using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using JezekT.NetStandard.Data;
using JezekT.NetStandard.Data.DataProviders.Repository;
using JezekT.NetStandard.Data.EntityOperations;
using JezekT.NetStandard.Services.EntityFrameworkCore.Resources;
using JezekT.NetStandard.Services.EntityOperations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JezekT.NetStandard.Services.EntityFrameworkCore.EntityOperations
{
    public class DeleteAssociationClassEntityWithValidationService<TEntity, FirstId, SecondId> : ValidationBase<TEntity>, IDeleteAssociationClassEntityWithValidation<TEntity, FirstId, SecondId>
        where TEntity : class, IAssociationClass<FirstId, SecondId>
    {
        private readonly IDeleteAssociationClassEntity<TEntity, FirstId, SecondId> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteAssociationClassEntityWithValidationService<TEntity, FirstId, SecondId>> _logger;

        public async Task<bool> DeleteByIdsAsync(FirstId firstObjId, SecondId secondObjId)
        {
            try
            {
                _repository.DeleteByIds(firstObjId, secondObjId);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                ExceptionMessage = ResourcesSettings.DeleteErrorMessage;
                _logger?.LogError(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                ExceptionMessage = ResourcesSettings.InvalidOperationMessage;
                _logger?.LogError(ex.Message);
            }
            return false;
        }


        public DeleteAssociationClassEntityWithValidationService(
            IDeleteAssociationClassEntity<TEntity, FirstId, SecondId> repository, IUnitOfWork unitOfWork, 
            ILogger<DeleteAssociationClassEntityWithValidationService<TEntity, FirstId, SecondId>> logger = null)
        {
            if (repository == null || unitOfWork == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            _repository = repository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
    }
}
