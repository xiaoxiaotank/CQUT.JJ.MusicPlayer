using System;
using System.Collections.Generic;
using System.Text;

namespace CQUT.JJ.MusicPlayer.Core.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string NickName { get; set; }
        public bool IsAdmin { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
