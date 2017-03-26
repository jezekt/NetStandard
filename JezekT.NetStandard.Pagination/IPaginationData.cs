namespace JezekT.NetStandard.Pagination
{
    public interface IPaginationData<T> where T : class 
    {
        T[] Items { get; set; }
        int RecordsTotal { get; set; }
        int RecordsFiltered { get; set; }
    }
}
