using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using JezekT.NetStandard.Data;
using JezekT.NetStandard.Pagination;
using JezekT.NetStandard.Pagination.DataProviders;

namespace JezekT.NetStandard.Services.EntityFrameworkCore.Pagination
{
    public abstract class PaginationServiceBase<TEntity, TItem, TId> 
        where TEntity : class, IWithId<TId>
        where TItem : class
    {
        private readonly IPaginationDataProvider<TEntity, TItem, TId> _paginationDataProvider;


        public async Task<IPaginationData<TItem>> GetPaginationDataAsync(int start, int pageSize, string term = null, string orderField = null,
            string orderDirection = null, TId[] inputFilterIds = null, TId[] skipIds = null)
        {
            return await _paginationDataProvider.GetPaginationDataAsync(start, pageSize, term, orderField, orderDirection, inputFilterIds, skipIds);
        }

        public async Task<IPaginationData<TItem>> GetPaginationDataAsync<TTemplate>(int start, int pageSize, string term = null, string orderField = null,
            string orderDirection = null, TId[] inputFilterIds = null, TId[] skipIds = null) where TTemplate : IPaginationTemplate<TEntity, TItem>
        {
            return await _paginationDataProvider.GetPaginationDataAsync<TTemplate>(start, pageSize, term, orderField, orderDirection, inputFilterIds, skipIds);
        }
        

        protected PaginationServiceBase(IPaginationDataProvider<TEntity, TItem, TId> paginationDataProvider)
        {
            if (paginationDataProvider == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            _paginationDataProvider = paginationDataProvider;
        }

    }
}
