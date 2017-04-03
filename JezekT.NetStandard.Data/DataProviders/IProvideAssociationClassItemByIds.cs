using System.Threading.Tasks;

namespace JezekT.NetStandard.Data.DataProviders
{
    public interface IProvideAssociationClassItemByIds<T, in FirstId, in SecondId>
        where T : class, IAssociationClass<FirstId, SecondId>
    {
        Task<T> GetByIdsAsync(FirstId firstObjId, SecondId secondObjId);

    }
}
