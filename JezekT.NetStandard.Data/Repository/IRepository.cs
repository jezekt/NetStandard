using System.Threading.Tasks;

namespace JezekT.NetStandard.Data.Repository
{
    public interface IRepository<T> where T : class 
    {
        Task<T> GetByIdAsync(int id);
        void Create(T jednotka);
        void Update(T jednotka);
        void DeleteById(int id);
    }
}
