using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.LocalLog
{
    public class LogViewModel
    {
        [JsonProperty("sId")]
        public int SId { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("dateTime")]
        public string DateTime { get; set; }
    }
}
