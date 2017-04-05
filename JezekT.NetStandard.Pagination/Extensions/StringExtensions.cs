using System;
using System.Linq;

namespace JezekT.NetStandard.Pagination.Extensions
{
    public static class StringExtensions
    {
        public static TId[] ToIdsArray<TId>(this string idsString)
        {
            if (string.IsNullOrEmpty(idsString))
            {
                return null;
            }
            if (idsString == "*")
            {
                return new TId[0];
            }
            return idsString.Split(',').Select(x => (TId)Convert.ChangeType(x, typeof(TId))).ToArray();
        }
    }
}
