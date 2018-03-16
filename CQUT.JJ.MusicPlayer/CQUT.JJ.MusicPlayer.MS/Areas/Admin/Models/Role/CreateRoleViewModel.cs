using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Role
{
    public class CreateRoleViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [Display(Name = "角色名")]
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("isDefault")]
        public bool IsDefault { get; set; }
        [JsonProperty("permissionCodes")]
        public string[] PermissionCodes { get; set; }
    }
}
