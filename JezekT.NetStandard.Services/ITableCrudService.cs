using JezekT.NetStandard.Pagination.DataProviders;

namespace JezekT.NetStandard.Services
{
    public interface ITableCrudService<T, in TId> : ICrudService<T, TId>, IPaginationDataProvider<TId>, IServiceErrorsProvider where T : class 
    {
    }
}
