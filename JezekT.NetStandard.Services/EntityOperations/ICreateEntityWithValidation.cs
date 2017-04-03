using System.Threading.Tasks;

namespace JezekT.NetStandard.Services.EntityOperations
{
    public interface ICreateEntityWithValidation<in TEntity> : IServiceErrorsProvider
        where TEntity : class 
    {
        Task<bool> CreateAsync(TEntity obj);

    }
}
