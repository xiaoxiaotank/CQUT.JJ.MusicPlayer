using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQUT.JJ.MusicPlayer.EntityFramework.Persistences.Extensions.PageExtension
{
    public class PagedList<T> : List<T>, IPagedList
    {
        public int PageCount { get; private set; }

        public int TotalItemCount { get; private set; }

        public int PageNumber { get; private set; }

        public int PageSize { get; private set; }

        public PagedList(List<T> items, int pageNumber, int pageSize, int pageCount, int totalItemCount)
        {
            PageNumber = pageNumber;
            PageSize = pageNumber;
            PageCount = PageCount;
            TotalItemCount = totalItemCount;
            AddRange(items);
        }
    }
}
