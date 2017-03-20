using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace JezekT.NetStandard.Pagination.EntityFrameworkCore.Extensions
{
    public static class QueryableOfTExtensions
    {
        private const string DescDirection = "desc";

        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string propertyName, string direction = null)
        {
            if (string.IsNullOrEmpty(propertyName)) throw new ArgumentNullException();
            Contract.EndContractBlock();

            // LAMBDA: x => x.[PropertyName]
            var parameter = Expression.Parameter(typeof(TSource), "x");
            Expression property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);

            // REFLECTION: source.OrderBy(x => x.Property)
            var order = direction == DescDirection ? "OrderByDescending" : "OrderBy";
            var orderByMethod = typeof(Queryable).GetRuntimeMethods().First(x => x.Name == order && x.GetParameters().Length == 2);
            var orderByGeneric = orderByMethod.MakeGenericMethod(typeof(TSource), property.Type);
            var result = orderByGeneric.Invoke(null, new object[] { source, lambda });

            return (IOrderedQueryable<TSource>)result;
        }

        public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, string direction)
        {
            if (keySelector == null) throw new ArgumentNullException();
            Contract.EndContractBlock();

            if (direction == DescDirection)
            {
                return source.OrderByDescending(keySelector);
            }
            return source.OrderBy(keySelector);
        }
    }
}
