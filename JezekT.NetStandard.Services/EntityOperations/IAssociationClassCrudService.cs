using System.Threading.Tasks;
using JezekT.NetStandard.Data;

namespace JezekT.NetStandard.Services.EntityOperations
{
    public interface IAssociationClassCrudService<T, in FirstId, in SecondId> : ICreateEntityWithValidation<T>, IUpdateEntityWithValidation<T>
        where T : class, IAssociationClass<FirstId, SecondId>
    {
        Task<T> GetByIdsAsync(FirstId firstObjId, SecondId secondObjId);
        Task<bool> DeleteByIdsAsync(FirstId firstObjId, SecondId secondObjId);

    }
}
