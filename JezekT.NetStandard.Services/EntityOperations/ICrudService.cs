using JezekT.NetStandard.Services.DataProviders;

namespace JezekT.NetStandard.Services.EntityOperations
{
    public interface ICrudService<T, in TId> : IProvideItemById<T, TId>, ICreateEntityWithValidation<T>, IUpdateEntityWithValidation<T>, 
        IDeleteEntityWithValidation<T, TId>
        where T : class 
    {
    }
}
