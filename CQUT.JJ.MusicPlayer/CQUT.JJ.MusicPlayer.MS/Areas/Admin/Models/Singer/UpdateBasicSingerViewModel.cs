using CQUT.JJ.MusicPlayer.MS.Utils.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Singer
{
    public class UpdateBasicSingerViewModel
    {
        public readonly IEnumerable<SelectListItem> Nationalities = null;

        private string _nationality = string.Empty;

        public int Id { get; set; }

        public string Name { get; set; }

        [DisplayName("外文名")]
        public string ForeignName { get; set; }

        [DisplayName("国籍")]
        public string Nationality
        {
            get => _nationality;
            set
            {
                _nationality = value;
                if (Nationalities?.Any() == true)
                {
                    if (value != null)
                    {
                        var nationality = Nationalities.SingleOrDefault(n => n.Value == value);
                        if (nationality != null)
                            nationality.Selected = true;
                    }
                }
                
            }
        }

        public UpdateBasicSingerViewModel()
        {
            Nationalities = GlobalHelper.GetNationalities()
                .Select(n => new SelectListItem()
                {
                    Text = n.Key,
                    Value = n.Value
                });
        }
    }
}
