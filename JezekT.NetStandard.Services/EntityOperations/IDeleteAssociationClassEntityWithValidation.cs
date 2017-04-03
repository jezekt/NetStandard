using System.Threading.Tasks;

namespace JezekT.NetStandard.Services.EntityOperations
{
    public interface IDeleteAssociationClassEntityWithValidation<TEntity, in FirstId, in SecondId> : IServiceErrorsProvider
        where TEntity : class
    {
        Task<bool> DeleteByIdsAsync(FirstId firstObjId, SecondId secondObjId);
    }
}
