using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using JezekT.NetStandard.Data;
using JezekT.NetStandard.Pagination.DataProviders;
using JezekT.NetStandard.Pagination.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JezekT.NetStandard.Pagination.EntityFrameworkCore.DataProviders
{
    public abstract class PaginationDataProviderBase<TEntity, TId, TContext, TItem> 
        where TEntity : class
        where TContext : DbContext
        where TItem : class
    {
        private readonly ILogger _logger;


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

        public async Task<IPaginationData<TItem>> GetPaginationDataAsync(int start, int pageSize, string term = null, string orderField = null,
            string orderDirection = null, Expression<Func<TEntity, bool>> inputFilterIdsExpression = null, Expression<Func<TEntity, bool>> skipIdsExpression = null)
        {
            if (DefaultPaginationTemplate == null)
            {
                throw new NotImplementedException();
            }

            return await GetPaginationResponseAsync(DefaultPaginationTemplate.GetSelector(), start, pageSize, orderField, orderDirection,
                inputFilterIdsExpression, skipIdsExpression, DefaultPaginationTemplate.GetSearchTermExpression(term));
        }

        public async Task<IPaginationData<TItem>> GetPaginationDataAsync<TTemplate>(int start, int pageSize, string term = null, string orderField = null,
            string orderDirection = null, Expression<Func<TEntity, bool>> inputFilterIdsExpression = null, Expression<Func<TEntity, bool>> skipIdsExpression = null)
            where TTemplate : IPaginationTemplate<TEntity, TItem>
        {
            var template = Activator.CreateInstance<TTemplate>();
            return await GetPaginationResponseAsync(template.GetSelector(), start, pageSize, orderField, orderDirection, inputFilterIdsExpression, skipIdsExpression, template.GetSearchTermExpression(term));
        }

        
        public async Task<IPaginationData<TItem>> GetPaginationResponseAsync(Expression<Func<TEntity, TItem>> selector, int start, int pageSize, string orderField = null,
            string orderDirection = null, Expression<Func<TEntity, bool>> inputFilterIdsExpression = null, Expression<Func<TEntity, bool>> skipIdsExpression = null,
            Expression<Func<TEntity, bool>> searchTermExpression = null)
        {
            if (inputFilterIdsExpression != null)
            {
                BaseQuery = BaseQuery.Where(inputFilterIdsExpression);
            }

            if (skipIdsExpression != null)
            {
                BaseQuery = BaseQuery.Where(skipIdsExpression);
            }

            var query = BaseQuery;
            if (searchTermExpression != null)
            {
                query = query.Where(searchTermExpression);
            }

            try
            {
                var totalCount = await BaseQuery.CountAsync();
                var response = await GetPaginationResponseAsync(start, pageSize, totalCount, orderField, orderDirection, query, selector);
                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.GetBaseException()?.Message?? ex.Message);
                throw;
            }
        }


        protected virtual IOrderedQueryable<TEntity> GetOrderedQueryable(string orderField, string orderDirection, IQueryable<TEntity> query)
        {
            return query.OrderBy(orderField, orderDirection);
        }
        

        protected PaginationDataProviderBase(TContext dbContext, ILogger logger = null)
        {
            if (dbContext == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            BaseQuery = dbContext.Set<TEntity>();
            _logger = logger;
        }

        protected PaginationDataProviderBase(IQueryable<TEntity> baseQuery, ILogger logger = null)
        {
            if (baseQuery == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            BaseQuery = baseQuery;
            _logger = logger;
        }


        private async Task<IPaginationData<TItem>> GetPaginationResponseAsync(Expression<Func<TEntity, TItem>> selector, int start, int pageSize,
            string orderField = null, string orderDirection = null, TId[] inputFilterIds = null, TId[] skipIds = null, Expression<Func<TEntity, bool>> searchTermExpression = null)
        {
            Expression<Func<TEntity, bool>> inputFilterIdsExpression = null;
            if (inputFilterIds != null)
            {
                if (typeof(IWithId<TId>).GetTypeInfo().IsAssignableFrom(typeof(TEntity).GetTypeInfo()))
                {
                    inputFilterIdsExpression = x => inputFilterIds.Contains(((IWithId<TId>)x).Id);
                }
            }

            Expression<Func<TEntity, bool>> skipIdsExpression = null;
            if (skipIds != null)
            {
                if (typeof(IWithId<TId>).GetTypeInfo().IsAssignableFrom(typeof(TEntity).GetTypeInfo()))
                {
                    skipIdsExpression = x => !skipIds.Contains(((IWithId<TId>)x).Id);
                }
            }

            return await GetPaginationResponseAsync(selector, start, pageSize, orderField, orderDirection, inputFilterIdsExpression, skipIdsExpression, searchTermExpression);
        }
        
        private async Task<IPaginationData<TItem>> GetPaginationResponseAsync(int start, int pageSize, int totalCount, string orderField, string orderDirection, IQueryable<TEntity> query, Expression<Func<TEntity, TItem>> selector)
        {
            if (!string.IsNullOrEmpty(orderField))
            {
                query = GetOrderedQueryable(orderField, orderDirection, query);
            }

            var items = await query.Select(selector).Skip(start).Take(pageSize).ToArrayAsync();
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
