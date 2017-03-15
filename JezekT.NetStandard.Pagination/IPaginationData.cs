namespace JezekT.NetStandard.Pagination
{
    public interface IPaginationData
    {
        object[] Items { get; set; }
        int RecordsTotal { get; set; }
        int RecordsFiltered { get; set; }
    }
}
