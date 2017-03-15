namespace JezekT.NetStandard.Data
{
    public interface IWithId<TId>
    {
        TId Id { get; set; }
    }
}
