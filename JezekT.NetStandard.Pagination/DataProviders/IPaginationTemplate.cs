using System;
using System.Linq.Expressions;

namespace JezekT.NetStandard.Pagination.DataProviders
{
    public interface IPaginationTemplate<TEntity, TItem> 
        where TEntity : class
        where TItem : class
    {
        Expression<Func<TEntity, TItem>> GetSelector();
        Expression<Func<TEntity, bool>> GetSearchTermExpression(string term);

    }
}
