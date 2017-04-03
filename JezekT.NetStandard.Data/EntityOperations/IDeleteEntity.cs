namespace JezekT.NetStandard.Data.EntityOperations
{
    public interface IDeleteEntity<TEntity, in TId>
        where TEntity : class 
    {
        void DeleteById(TId id);
    }
}
