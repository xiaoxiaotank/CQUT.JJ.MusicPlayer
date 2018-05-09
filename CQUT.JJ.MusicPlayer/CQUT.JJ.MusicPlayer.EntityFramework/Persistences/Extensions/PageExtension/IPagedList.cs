using System;
using System.Collections.Generic;
using System.Text;

namespace CQUT.JJ.MusicPlayer.EntityFramework.Persistences.Extensions.PageExtension
{

    public interface IPagedList
    {
        int PageCount { get; }

        int TotalItemCount { get; }

        int PageNumber { get; }

        int PageSize { get; }
    }

}
