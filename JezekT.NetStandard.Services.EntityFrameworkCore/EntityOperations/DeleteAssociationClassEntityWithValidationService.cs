using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using JezekT.NetStandard.Data;
using JezekT.NetStandard.Data.DataProviders.Repository;
using JezekT.NetStandard.Data.EntityOperations;
using JezekT.NetStandard.Services.EntityFrameworkCore.Resources;
using JezekT.NetStandard.Services.EntityOperations;
using Microsoft.EntityFrameworkCore;

namespace JezekT.NetStandard.Services.EntityFrameworkCore.EntityOperations
{
    public class DeleteAssociationClassEntityWithValidationService<TEntity, FirstId, SecondId> : ValidationBase<TEntity>, IDeleteAssociationClassEntityWithValidation<TEntity, FirstId, SecondId>
        where TEntity : class, IAssociationClass<FirstId, SecondId>
    {
        private readonly IDeleteAssociationClassEntity<TEntity, FirstId, SecondId> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public async Task<bool> DeleteByIdsAsync(FirstId firstObjId, SecondId secondObjId)
        {
            try
            {
                _repository.DeleteByIds(firstObjId, secondObjId);
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


        public DeleteAssociationClassEntityWithValidationService(
            IDeleteAssociationClassEntity<TEntity, FirstId, SecondId> repository, IUnitOfWork unitOfWork)
        {
            if (repository == null || unitOfWork == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            _repository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
