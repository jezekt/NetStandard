using System.Threading.Tasks;

namespace JezekT.NetStandard.Services
{
    public interface ICrudService<T, in TId> : IServiceErrorsProvider
        where T : class 
    {
        Task<T> GetByIdAsync(TId id);
        Task<bool> CreateAsync(T obj);
        Task<bool> UpdateAsync(T obj);
        Task<bool> DeleteByIdAsync(TId id);
    }
}
