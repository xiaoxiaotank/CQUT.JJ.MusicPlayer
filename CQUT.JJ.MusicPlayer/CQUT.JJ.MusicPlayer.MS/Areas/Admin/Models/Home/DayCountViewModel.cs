using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Home
{
    public class DayCountViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public Dictionary<DateTime,int> Value { get; set; }
    }
}
