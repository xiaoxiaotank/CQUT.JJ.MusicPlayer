using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Entities
{
    public class LogItemEntity
    {
        public string Message { get; set; }

        public DateTime DateTime { get; set; }

        public string UserName { get; set; }

        public LogType Type { get; set; }

        public string Source { get; set; }

        public LogItemEntity(string message,string userName,LogType type,string source)
        {
            DateTime = DateTime.Now;
            Message = message;
            UserName = userName;
            Type = type;
            Source = source;
        }

        public LogItemEntity() { }
    }
}
