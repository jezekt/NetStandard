using System.Threading.Tasks;

namespace JezekT.NetStandard.Data.DataProviders.Repository
{
    public interface IUnitOfWork
    {
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
