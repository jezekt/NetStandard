namespace JezekT.NetStandard.Data
{
    public interface IAssociationClass<out FirstId, out SecondId>
    {
        FirstId FirstObjId { get; }
        SecondId SecondObjId { get; }
    }
}
