namespace JezekT.NetStandard.Pagination
{
    public class PaginationResponse<T> : IPaginationData<T>
        where T : class 
    {
        public T[] Items { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
    }
}
