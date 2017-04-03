using System.Threading.Tasks;

namespace JezekT.NetStandard.Services.EntityOperations
{
    public interface IUpdateEntityWithValidation<in T> : IServiceErrorsProvider
        where T : class
    {
        Task<bool> UpdateAsync(T obj);
    }
}
