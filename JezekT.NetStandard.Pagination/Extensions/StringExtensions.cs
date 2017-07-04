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

            var stringIds = idsString.Split(',');
            if (typeof(TId) == typeof(Guid))
            {
                return stringIds.Select(x => new Guid(x)).OfType<TId>().ToArray();
            }
            return stringIds.Select(x => (TId)Convert.ChangeType(x, typeof(TId))).ToArray();
        }
    }
}
