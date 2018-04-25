using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Home
{
    public class UserCountViewModel
    {
        [JsonProperty("memberCount")]
        public int MemberCount { get; set; }

        [JsonProperty("todayRegisterMemberCount")]
        public int TodayRegisterMemberCount { get; set; }

        [JsonProperty("employeeCount")]
        public int EmployeeCount { get; set; }

        [JsonProperty("todayCreateEmployeeCount")]
        public int TodayCreateEmployeeCount{ get; set; }
    }
}
