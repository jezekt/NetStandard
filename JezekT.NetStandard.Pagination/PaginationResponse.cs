namespace JezekT.NetStandard.Pagination
{
    public class PaginationResponse : IPaginationData
    {
        public object[] Items { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
    }
}
