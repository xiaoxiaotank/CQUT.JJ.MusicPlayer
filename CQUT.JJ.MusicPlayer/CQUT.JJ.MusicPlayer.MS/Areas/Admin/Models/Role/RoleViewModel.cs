using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Role
{
    public class RoleViewModel
    {
        [JsonProperty("sId")]
        public int SId { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("isDefault")]
        public bool IsDefault { get; set; }
        [JsonProperty("lastModificationTime")]
        public string LastModificationTime { get; set; }
        [JsonProperty("creationTime")]
        public string CreationTime { get; set; }
    }
}
