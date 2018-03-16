using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Role
{
    public class UpdateRoleViewModel
    {
        public int Id { get; set; }
        public bool IsDefault { get; set; }
        public string Name { get; set; }
    }
}
