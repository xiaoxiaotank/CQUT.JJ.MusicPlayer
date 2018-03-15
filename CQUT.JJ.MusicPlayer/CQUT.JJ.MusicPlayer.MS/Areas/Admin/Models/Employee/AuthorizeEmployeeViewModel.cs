using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Employee
{
    public class AuthorizeEmployeeViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("permissionCodes")]
        public string[] PermissionCodes { get; set; }
    }
}
