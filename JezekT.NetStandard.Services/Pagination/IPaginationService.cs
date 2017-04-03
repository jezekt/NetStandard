using JezekT.NetStandard.Data;
using JezekT.NetStandard.Pagination.DataProviders;

namespace JezekT.NetStandard.Services.Pagination
{
    public interface IPaginationService<TEntity, in TId, TPaginationItem> : IPaginationDataProvider<TEntity, TPaginationItem, TId> 
        where TEntity : class, IWithId<TId>
        where TPaginationItem : class
    {
    }
}
