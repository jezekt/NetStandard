using System;
using System.Linq.Expressions;
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

    public interface IPaginationDataProvider<TEntity, TItem>
        where TEntity : class
        where TItem : class
    {
        Task<IPaginationData<TItem>> GetPaginationDataAsync(int start, int pageSize, string term = null, string orderField = null, string orderDirection = null,
            Expression<Func<TEntity, bool>> inputFilterIdsExpression = null, Expression<Func<TEntity, bool>> skipIdsExpression = null);

        Task<IPaginationData<TItem>> GetPaginationDataAsync<TTemplate>(int start, int pageSize, string term = null, string orderField = null, string orderDirection = null,
            Expression<Func<TEntity, bool>> inputFilterIdsExpression = null, Expression<Func<TEntity, bool>> skipIdsExpression = null)
            where TTemplate : IPaginationTemplate<TEntity, TItem>;

    }
}
