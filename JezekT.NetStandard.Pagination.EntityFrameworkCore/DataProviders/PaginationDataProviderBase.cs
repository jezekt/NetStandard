using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JezekT.NetStandard.Data;
using JezekT.NetStandard.Pagination.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;

namespace JezekT.NetStandard.Pagination.EntityFrameworkCore.DataProviders
{
    public abstract class PaginationDataProviderBase<TEntity, TId, TContext> 
        where TEntity : class, IWithId<TId>
        where TContext : DbContext
    {
        protected IQueryable<TEntity> BaseQuery { get; private set; }

        protected abstract Expression<Func<TEntity, object>> GetSelector();
        protected abstract Expression<Func<TEntity, bool>> GetSearchTermExpression(string term);


        public async Task<IPaginationData> GetPaginationDataAsync(string term, int start, int pageSize, string orderField, string orderDirection, TId[] inputFiletrIds = null)
        {
            return await GetPaginationResponseAsync(GetSelector(), start, pageSize, orderField, orderDirection, inputFiletrIds, GetSearchTermExpression(term));
        }
        
        protected async Task<IPaginationData> GetPaginationResponseAsync(Expression<Func<TEntity, object>> selector,
            int start, int pageSize, string orderField = null, string orderDirection = null, TId[] inputFiletrIds = null,
            Expression<Func<TEntity, bool>> searchTermExpression = null)
        {
            if (inputFiletrIds != null)
            {
                BaseQuery = BaseQuery.Where(x => inputFiletrIds.Contains(x.Id));
            }

            var query = BaseQuery;
            if (searchTermExpression != null)
            {
                query = query.Where(searchTermExpression);
            }

            var totalCount = await BaseQuery.CountAsync();
            var response = await GetResponseAsync(start, pageSize, totalCount, orderField, orderDirection, query, selector);
            return response;
        }


        protected PaginationDataProviderBase(TContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            BaseQuery = dbContext.Set<TEntity>();
        }


        private async Task<IPaginationData> GetResponseAsync(int start, int pageSize, int totalCount, string orderField, string orderDirection, IQueryable<TEntity> query, Expression<Func<TEntity, object>> selector)
        {
            if (!string.IsNullOrEmpty(orderField))
            {
                query = query.OrderBy(orderField, orderDirection);
            }

            var items = await query.Skip(start).Take(pageSize).Select(selector).ToArrayAsync();
            var count = await query.CountAsync();

            return new PaginationResponse
            {
                Items = items,
                RecordsTotal = totalCount,
                RecordsFiltered = count,
            };
        }


    }
}
