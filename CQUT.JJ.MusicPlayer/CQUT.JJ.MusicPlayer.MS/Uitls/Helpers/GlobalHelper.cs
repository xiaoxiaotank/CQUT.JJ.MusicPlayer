using CQUT.JJ.MusicPlayer.Core.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Utils.Helpers
{
    public static class GlobalHelper
    {
        public static UserModel GetCurrentUser(this ISession session)
        {
            var jsonValue = session.GetString("User");
            var user = jsonValue != null ? JsonConvert.DeserializeObject<UserModel>(jsonValue) : default(UserModel);
            return user;
        }
    }
}
