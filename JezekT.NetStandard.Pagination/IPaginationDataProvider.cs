using System.Threading.Tasks;
using JezekT.NetStandard.Data;

namespace JezekT.NetStandard.Pagination
{
    public interface IPaginationDataProvider
    {
        Task<IPaginationData> GetPaginationDataAsync(string term, int start, int pageSize, string orderField, string orderDirection);
    }

    public interface IPaginationDataProvider<TEntity, in TId> where TEntity : class, IWithId<TId>
    {
        Task<IPaginationData> GetPaginationDataAsync(string term, int start, int pageSize, string orderField, string orderDirection, TId[] inputFiletrIds = null);
    }

}
