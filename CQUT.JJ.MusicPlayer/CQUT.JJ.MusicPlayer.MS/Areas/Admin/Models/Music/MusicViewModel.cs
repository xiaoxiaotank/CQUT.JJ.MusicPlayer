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

        [JsonProperty("creatorId")]
        public int CreatorId { get; set; }
        [JsonProperty("menderId")]
        public int? MenderId { get; set; }
        [JsonProperty("publisherId")]
        public int PublisherId { get; set; }
        [JsonProperty("unpublisherId")]
        public int? UnpublisherId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("singerName")]
        public string SingerName { get; set; }

        [JsonProperty("albumName")]
        public string AlbumName { get; set; }
        [JsonProperty("creatorName")]
        public string CreatorName { get; set; }
        [JsonProperty("menderName")]
        public string MenderName { get; set; }
        [JsonProperty("publisherName")]
        public string PublisherName { get; set; }
        [JsonProperty("unpublisherName")]
        public string UnpublisherName { get; set; }
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
