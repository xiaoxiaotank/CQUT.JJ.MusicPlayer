using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Member
{
    public class MemberViewModel
    {
        public int id { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int sId { get; set; }

        public string userName { get; set; }

        public string nickName { get; set; }

        public string creationTime { get; set; }
    }
}
