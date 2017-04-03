namespace JezekT.NetStandard.Data.EntityOperations
{
    public interface IUpdateEntity<in T>
        where T : class
    {
        void Update(T obj);

    }
}
