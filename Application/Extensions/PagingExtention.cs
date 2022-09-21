using DataLayer.DTOs.Paging;
using System.Linq;

namespace Application.Extensions
{
    public static class PagingExtention
    {
        public static IQueryable<T> Paging<T>(this IQueryable<T> query , BasePaging paging)
        {
            return query.Skip(paging.SkipEntity).Take(paging.TakeEntity);
        }
    }
}
