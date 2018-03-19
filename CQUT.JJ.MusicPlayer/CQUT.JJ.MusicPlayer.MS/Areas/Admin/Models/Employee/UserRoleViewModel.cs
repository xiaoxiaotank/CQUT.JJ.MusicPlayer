using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Employee
{
    public class UserRoleViewModel
    {
        [JsonProperty("sId")]
        public int SId { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("roleName")]
        public string RoleName { get; set; }
        [JsonProperty("isDefault")]
        public bool IsDefault { get; set; }
        [JsonProperty("hasOwned")]
        public bool HasOwned { get; set; }
    }
}
