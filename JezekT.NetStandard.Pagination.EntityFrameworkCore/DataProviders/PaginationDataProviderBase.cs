using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JezekT.NetStandard.Data;
using JezekT.NetStandard.Pagination.DataProviders;
using JezekT.NetStandard.Pagination.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;

namespace JezekT.NetStandard.Pagination.EntityFrameworkCore.DataProviders
{
    public abstract class PaginationDataProviderBase<TEntity, TId, TContext, TItem> 
        where TEntity : class, IWithId<TId>
        where TContext : DbContext
        where TItem : class
    {
        protected IQueryable<TEntity> BaseQuery { get; private set; }
        protected abstract IPaginationTemplate<TEntity, TItem> DefaultPaginationTemplate { get; }


        public async Task<IPaginationData<TItem>> GetPaginationDataAsync(int start, int pageSize, string term = null, string orderField = null,
            string orderDirection = null, TId[] inputFilterIds = null, TId[] skipIds = null)
        {
            if (DefaultPaginationTemplate == null)
            {
                throw new NotImplementedException();
            }

            return await GetPaginationResponseAsync(DefaultPaginationTemplate.GetSelector(), start, pageSize, orderField, orderDirection, 
                inputFilterIds, skipIds, DefaultPaginationTemplate.GetSearchTermExpression(term));
        }

        public async Task<IPaginationData<TItem>> GetPaginationDataAsync<TTemplate>(int start, int pageSize, string term = null, string orderField = null,
            string orderDirection = null, TId[] inputFilterIds = null, TId[] skipIds = null)
            where TTemplate : IPaginationTemplate<TEntity, TItem>
        {
            var template = Activator.CreateInstance<TTemplate>();
            return await GetPaginationResponseAsync(template.GetSelector(), start, pageSize, orderField, orderDirection, inputFilterIds, skipIds, template.GetSearchTermExpression(term));
        }
        
        
        private async Task<IPaginationData<TItem>> GetPaginationResponseAsync(Expression<Func<TEntity, TItem>> selector, int start, int pageSize, 
            string orderField = null, string orderDirection = null, TId[] inputFilterIds = null, TId[] skipIds = null, Expression<Func<TEntity, bool>> searchTermExpression = null)
        {
            if (inputFilterIds != null)
            {
                BaseQuery = BaseQuery.Where(x => inputFilterIds.Contains(x.Id));
            }

            if (skipIds != null)
            {
                BaseQuery = BaseQuery.Where(x => !skipIds.Contains(x.Id));
            }

            var query = BaseQuery;
            if (searchTermExpression != null)
            {
                query = query.Where(searchTermExpression);
            }

            var totalCount = await BaseQuery.CountAsync();
            var response = await GetPaginationResponseAsync(start, pageSize, totalCount, orderField, orderDirection, query, selector);
            return response;
        }

        protected virtual IOrderedQueryable<TEntity> GetOrderedQueryable(string orderField, string orderDirection, IQueryable<TEntity> query)
        {
            return query.OrderBy(orderField, orderDirection);
        }



        protected PaginationDataProviderBase(TContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            BaseQuery = dbContext.Set<TEntity>();
        }


        private async Task<IPaginationData<TItem>> GetPaginationResponseAsync(int start, int pageSize, int totalCount, string orderField, string orderDirection, IQueryable<TEntity> query, Expression<Func<TEntity, TItem>> selector)
        {
            if (!string.IsNullOrEmpty(orderField))
            {
                query = GetOrderedQueryable(orderField, orderDirection, query);
            }

            var items = await query.Skip(start).Take(pageSize).Select(selector).ToArrayAsync();
            var count = await query.CountAsync();

            return new PaginationResponse<TItem>
            {
                Items = items,
                RecordsTotal = totalCount,
                RecordsFiltered = count
            };
        }


    }
}
