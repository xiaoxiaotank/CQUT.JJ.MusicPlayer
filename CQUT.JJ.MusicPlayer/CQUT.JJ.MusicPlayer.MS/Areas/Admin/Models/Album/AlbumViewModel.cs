using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Album
{
    public class AlbumViewModel
    {
        [JsonProperty("sId")]
        public int SId { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("singerId")]
        public int SingerId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("singerName")]
        public string SingerName { get; set; }
        [JsonProperty("creationTime")]
        public string CreationTime { get; set; }
        [JsonProperty("lastModificationTime")]
        public string LastModificationTime { get; set; }
        [JsonProperty("publishmentTime")]
        public string PublishmentTime { get; set; }
        [JsonProperty("deletionTime")]
        public string DeletionTime { get; set; }
    }
}
