using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JezekT.NetStandard.Data.DataProviders
{
    public interface IProvideItemByIdWithIncludes<T, in TId>
        where T : class
    {
        Task<T> GetByIdAsync(TId id, params Expression<Func<T, object>>[] includes);
    }
}
