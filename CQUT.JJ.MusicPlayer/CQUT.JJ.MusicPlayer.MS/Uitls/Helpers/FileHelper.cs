using CQUT.JJ.MusicPlayer.Core.Models;
using CQUT.JJ.MusicPlayer.EntityFramework.Persistences;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Uitls.Helpers
{
    public static class FileHelper
    {
        public static string GetFullFileNameOfMusic(string contentDisposition,SingerModel singer,AlbumModel album)
        {
            var fileName = ContentDispositionHeaderValue
                    .Parse(contentDisposition)
                    .FileName
                    .Trim('"');
            return Path.Combine(GlobalConstants.MusicsRootPath, GlobalConstants.MusicsRootDirectoryName, 
                $"{singer.Id.ToString()}-{singer.Name}", album.Name, $"{DateTime.Now.ToString("yyyyMMddHHmmss")}-{fileName}");
        }

        public static bool SaveTo(this IFormFile file, string fileName)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                using (var fs = File.Create(fileName))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
