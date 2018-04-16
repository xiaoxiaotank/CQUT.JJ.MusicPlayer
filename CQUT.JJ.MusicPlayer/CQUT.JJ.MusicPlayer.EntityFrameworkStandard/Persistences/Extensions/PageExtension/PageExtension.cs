using System;
using System.Linq;

namespace CQUT.JJ.MusicPlayer.EntityFramework.Persistences.Extensions.PageExtension
{
    public static class PageExtension
    {
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> items,int page,int size)
        {
            if (page < 1)
                page = 1;
            var pageOfItems = items.Skip((page - 1) * size).Take(size)?.ToList();
            var totalItemCount = items.Count();
            var pageCount = (int)Math.Ceiling((double)totalItemCount / size);
            return new PagedList<T>(pageOfItems, page, size, pageCount, totalItemCount);
        }
    }
}
