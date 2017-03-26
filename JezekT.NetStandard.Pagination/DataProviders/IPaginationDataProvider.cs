using System.Threading.Tasks;
using JezekT.NetStandard.Data;

namespace JezekT.NetStandard.Pagination.DataProviders
{
    public interface IPaginationDataProvider<TEntity, TItem, in TId>
        where TEntity : class, IWithId<TId>
        where TItem : class
    {
        Task<IPaginationData<TItem>> GetPaginationDataAsync(int start, int pageSize, string term = null, string orderField = null,
            string orderDirection = null, TId[] inputFilterIds = null, TId[] skipIds = null);

        Task<IPaginationData<TItem>> GetPaginationDataAsync<TTemplate>(int start, int pageSize, string term = null, string orderField = null,
            string orderDirection = null, TId[] inputFilterIds = null, TId[] skipIds = null) 
            where TTemplate : IPaginationTemplate<TEntity, TItem>;

    }
}
