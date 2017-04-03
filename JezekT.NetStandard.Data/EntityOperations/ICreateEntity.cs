namespace JezekT.NetStandard.Data.EntityOperations
{
    public interface ICreateEntity<in T>
        where T : class 
    {
        void Create(T obj);
    }
}
