namespace JezekT.NetStandard.Data.EntityOperations
{
    public interface IDeleteAssociationClassEntity<TEtnity, in FirstId, in SecondId>
        where TEtnity : class, IAssociationClass<FirstId, SecondId>
    {
        void DeleteByIds(FirstId firstObjId, SecondId secondObjId);
    }
}
