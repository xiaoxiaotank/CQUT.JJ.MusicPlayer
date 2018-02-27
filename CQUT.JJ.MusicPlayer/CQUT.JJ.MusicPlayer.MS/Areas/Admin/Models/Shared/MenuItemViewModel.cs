using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Shared
{
    public class MenuItemViewModel
    {
        public int Id { set; get; }
        public int ParentId { set; get; }
        public string Header { get; set; }
        public string TargetUrl { get; set; }
        public string RequiredAuthorizeCode { get; set; }
        public int Priority { get; set; }
        public List<MenuItemViewModel> Children { set; get; }
        public bool HasChild => Children.Any();

        public MenuItemViewModel()
        {
            Children = new List<MenuItemViewModel>();
        }
    }
}
