﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Role
{
    public class UpdateRoleViewModel
    {
        public int Id { get; set; }
        [DisplayName("角色名")]
        public string Name { get; set; }
        public bool IsDefault { get; set; }
    }
}
