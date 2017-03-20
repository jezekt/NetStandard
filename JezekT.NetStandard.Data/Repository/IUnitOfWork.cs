using System.Threading.Tasks;

namespace JezekT.NetStandard.Data.Repository
{
    public interface IUnitOfWork
    {
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
