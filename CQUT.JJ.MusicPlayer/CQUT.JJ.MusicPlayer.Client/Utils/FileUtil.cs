using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
        /// <param name="fileName"></param>
        /// <returns>文件保存路径</returns>
        public static string SaveFileToLocal(string localPath, string serverPath, string fileName)
        {
            var Now = DateTime.Now;
            string relevantPath = $"{Now.ToString("yyyyMMdd")}";
            string folderPath = System.IO.Path.Combine(serverPath, relevantPath);
            string fliePath = System.IO.Path.Combine(folderPath
               , System.IO.Path.GetFileName($"{Now.ToString("yyyyMMddHHmmss")}{new Random().Next(100000, 999999999)}{fileName}")
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

        /// <summary>
        /// 按行写入文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="datas"></param>
        /// <returns>是否成功</returns>
        public static bool WriteToFileByLine(string path,string[] datas)
        {
            try
            {
                using (var fs = File.OpenWrite(path))
                using (var sw = new StreamWriter(fs))
                {
                    foreach (var data in datas)
                    {
                        sw.WriteLine(data);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 按行读文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IEnumerable<string> ReadFromFileByLine(string path)
        {
            using (var fs = File.OpenRead(path))
            using(var sr = new StreamReader(fs))
            {
                var data = string.Empty;
                while((data = sr.ReadLine()) != null)
                {
                    yield return data;
                }
            }
        }

        /// <summary>
        /// 音乐下载
        /// </summary>
        /// <param name="addressUrl"></param>
        /// <param name="localName"></param>
        public static async Task DownLoadMusicsAsync(Uri fileUri, string musicName,string singerName)
        {
            await Task.Factory.StartNew(() =>
            {
                //Thread.Sleep(10000);
                var addressUri = fileUri;
                if (!addressUri.IsAbsoluteUri)
                    addressUri = new Uri(Path.GetFullPath(fileUri.OriginalString),UriKind.Absolute);
                WebClient webClient = new WebClient();
                Directory.CreateDirectory("./Download");
                webClient.DownloadFileAsync(addressUri, $"./Download/{singerName}-{musicName}");
            });
        }
    }
}
