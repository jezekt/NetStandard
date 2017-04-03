using System.Threading.Tasks;

namespace JezekT.NetStandard.Data.DataProviders
{
    public interface IProvideItemById<T, in TId>
        where T : class
    {
        Task<T> GetByIdAsync(TId id);
    }
}
