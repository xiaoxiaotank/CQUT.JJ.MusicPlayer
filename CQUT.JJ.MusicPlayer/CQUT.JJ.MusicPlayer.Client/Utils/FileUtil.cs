using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Client.Utils
{
    public static class FileUtil
    {
        /// <summary>
        /// 存储文件到服务器
        /// </summary>
        /// <param name="localPath"></param>
        /// <param name="serverPath"></param>
        /// <param name="imageName"></param>
        /// <returns>文件保存路径</returns>
        public static string SaveFileToLocal(string localPath, string serverPath, string imageName)
        {
            var Now = DateTime.Now;
            string relevantPath = $"{Now.ToString("yyyyMMdd")}";
            string folderPath = System.IO.Path.Combine(serverPath, relevantPath);
            string fliePath = System.IO.Path.Combine(folderPath
               , System.IO.Path.GetFileName($"{Now.ToString("yyyyMMddHHmmss")}{new Random().Next(100000, 999999999)}{imageName}")
            );

            try
            {
                Directory.CreateDirectory(folderPath);
               new WebClient().UploadFile(fliePath, "Post", localPath);
            }
            catch
            {
                return string.Empty;
            }
            return fliePath;            
        }
    }
}
