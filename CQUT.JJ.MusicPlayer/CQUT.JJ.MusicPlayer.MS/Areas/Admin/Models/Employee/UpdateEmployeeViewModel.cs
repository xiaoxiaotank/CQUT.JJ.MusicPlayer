using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Employee
{
    public class UpdateEmployeeViewModel
    {
        public int Id { get; set; }

        [Display(Name = "用户名")]

        public string UserName { get; set; }

        [Display(Name = "昵称")]
        public string NickName { get; set; }

        
    }
}
