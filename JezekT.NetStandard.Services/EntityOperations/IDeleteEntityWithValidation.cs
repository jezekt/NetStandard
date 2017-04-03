using System.Threading.Tasks;

namespace JezekT.NetStandard.Services.EntityOperations
{
    public interface IDeleteEntityWithValidation<T, in TId> : IServiceErrorsProvider
        where T : class
    {
        Task<bool> DeleteByIdAsync(TId id);
    }
}
