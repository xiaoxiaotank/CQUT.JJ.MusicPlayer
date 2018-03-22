using CQUT.JJ.MusicPlayer.MS.Utils.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Singer
{
    public class CreateSingerViewModel
    {
        public readonly IEnumerable<SelectListItem> Nationalities = null;

        [DisplayName("姓名")]
        public string Name { get; set; }

        [DisplayName("外文名")]
        public string ForeignName { get; set; }

        [DisplayName("国籍")]
        public string Nationality { get; set; }

        public CreateSingerViewModel()
        {
            Nationalities = GlobalHelper.GetNationalities()
                .Select(n => new SelectListItem()
                {
                    Text = n.Key,
                    Value = n.Value
                });
            if(Nationalities.Any())
                Nationalities.FirstOrDefault().Selected = true;
        }

    }
}
