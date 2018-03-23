using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Music
{
    public class MusicViewModel
    {
        [JsonProperty("sId")]
        public int SId { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("singerId")]
        public int SingerId { get; set; }

        [JsonProperty("albumId")]
        public int AlbumId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("singerName")]
        public string SingerName { get; set; }

        [JsonProperty("albumName")]
        public string AlbumName { get; set; }

        [JsonProperty("duration")]
        public string Duration { get; set; }

        [JsonProperty("fileUrl")]
        public string FileUrl { get; set; }

        [JsonProperty("creationTime")]
        public string CreationTime { get; set; }

        [JsonProperty("lastModificationTime")]
        public string LastModificationTime { get; set; }

        [JsonProperty("publishmentTime")]
        public string PublishmentTime { get; set; }
    }
}
