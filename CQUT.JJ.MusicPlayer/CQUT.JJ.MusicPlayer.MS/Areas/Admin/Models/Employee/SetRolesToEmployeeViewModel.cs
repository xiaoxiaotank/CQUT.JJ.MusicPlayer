using CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Role;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Employee
{
    public class SetRolesToEmployeeViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        public string UserName { get; set; }

        [JsonProperty("roleIds")]
        public int[] RoleIds { get; set; }
    }
}
