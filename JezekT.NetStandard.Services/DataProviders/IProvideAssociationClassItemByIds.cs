using System.Threading.Tasks;

namespace JezekT.NetStandard.Services.DataProviders
{
    public interface IProvideAssociationClassItemByIds<T, in FirstId, in SecondId>
        where T : class 
    {
        Task<T> GetByIdsAsync(FirstId firstObjId, SecondId secondObjId);
    }
}
