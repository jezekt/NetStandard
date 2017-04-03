using JezekT.NetStandard.Data.EntityOperations;

namespace JezekT.NetStandard.Data.DataProviders.Repository
{
    public interface IRepository<T, in TId> : IProvideItemById<T, TId>, ICreateEntity<T>, IUpdateEntity<T>, IDeleteEntity<T, TId> 
        where T : class 
    {
    }
}
