using System.Linq;

namespace JezekT.NetStandard.Pagination.Extensions
{
    public static class ArrayOfTExtensions
    {
        public static string ToIdsString<TId>(this TId[] ids)
        {
            if (ids != null)
            {
                if (ids.Any())
                {
                    return string.Join(",", ids);
                }
                return "*";
            }
            return null;
        }
    }
}
