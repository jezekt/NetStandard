using System.Threading.Tasks;

namespace JezekT.NetStandard.Data.DataProviders.Repository
{
    public interface IAssociationClassRepository<T, in FirstId, in SecondId> where T : class, IAssociationClass<FirstId, SecondId>
    {
        Task<T> GetByIdsAsync(FirstId objAId, SecondId objBId);
        void Create(T obj);
        void Update(T obj);
        void DeleteByIds(FirstId objAId, SecondId objBId);
    }
}
