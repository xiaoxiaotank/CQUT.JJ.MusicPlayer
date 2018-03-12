using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Entities
{
    public class JsonResultEntity
    {
        /// <summary>
        /// 发送的消息
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        /// <summary>
        /// 标识是否成功
        /// 默认值为true
        /// </summary>
        [JsonProperty(PropertyName = "isSuccessed")]
        public bool IsSuccessed { get; set; } = true;

        /// <summary>
        /// json对象
        /// </summary>
        [JsonProperty(PropertyName = "jsonObject")]
        public JsonResult JsonObject { get; set; }

    }
}
