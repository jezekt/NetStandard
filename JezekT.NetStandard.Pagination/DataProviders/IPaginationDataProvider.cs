using System.Threading.Tasks;
using JezekT.NetStandard.Data;

namespace JezekT.NetStandard.Pagination.DataProviders
{
    public interface IPaginationDataProvider
    {
        Task<IPaginationData> GetPaginationDataAsync(string term, int start, int pageSize, string orderField, string orderDirection);
    }

    public interface IPaginationDataProvider<TEntity, in TId> : IPaginationDataProvider<TId> where TEntity : class, IWithId<TId>
    {
    }

    public interface IPaginationDataProvider<in TId>
    {
        Task<IPaginationData> GetPaginationDataAsync(string term, int start, int pageSize, string orderField, string orderDirection, TId[] inputFiletrIds = null);
    }


}
