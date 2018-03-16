using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Employee
{
    public class EmployeeViewModel
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [JsonProperty(PropertyName = "sId")]
        public int SId { get; set; }

        [JsonProperty(PropertyName = "userName")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "nickName")]
        public string NickName { get; set; }

        [JsonProperty(PropertyName = "creationTime")]
        public string CreationTime { get; set; }

        [JsonProperty(PropertyName = "lastModificationTime")]
        public string LastModificationTime { get; set; }

        [JsonProperty("isSuperManager")]
        public bool IsSuperManager { get; set; }
    }
}
