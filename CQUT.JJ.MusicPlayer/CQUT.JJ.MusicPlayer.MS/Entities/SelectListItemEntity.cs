using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Entities
{
    public class SelectListItemEntity
    {
        public SelectListItem Item { get; set; }

        public string ParentId { get; set; }
    }
}
