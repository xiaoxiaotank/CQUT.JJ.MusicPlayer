using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CQUT.JJ.MusicPlayer.Client.Utils.Configs.HistoryPlayList
{
    public static class HistoryPlayListUtil
    {
        private const string History_Root_Element = "HistoryPlayList";

        private const string History_Item_Element = "HistoryPlayItem";

        private static readonly string _historyFullDirectory = Path.Combine(Environment.CurrentDirectory, "Configs");

        private static readonly string _historyPath = Path.Combine(_historyFullDirectory, "HistoryPlayList.xml");

        private static readonly Object _obj = new object();

        /// <summary>
        /// 存储历史播放列表
        /// </summary>
        /// <param name="entity"></param>
        public static void SaveHistoryPlayItem(HistoryPlayItemEntity entity)
        {
            XElement xmlRoot = null;
            Directory.CreateDirectory(_historyFullDirectory);
            lock (_obj)
            {
                if (!File.Exists(_historyPath))
                    xmlRoot = CreateXML().Element(History_Root_Element);
                if (xmlRoot == null)
                    xmlRoot = GetLogRootElement();

                var element = xmlRoot.Elements().SingleOrDefault(e => e.Element(nameof(entity.MusicId))?.Value == entity.MusicId.ToString());
                if (element != null)
                {
                    element.Remove();
                    xmlRoot.Add(element);
                }
                else
                {
                    var log = new XElement(History_Item_Element,
                    new XElement(nameof(entity.MusicId), entity.MusicId)
                    , new XElement(nameof(entity.SingerId), entity.SingerId)
                    , new XElement(nameof(entity.AlbumId), entity.AlbumId)
                    , new XElement(nameof(entity.MusicName), entity.MusicName)
                    , new XElement(nameof(entity.SingerName), entity.SingerName)
                    , new XElement(nameof(entity.AlbumName), entity.AlbumName)
                    , new XElement(nameof(entity.MusicFileUri), entity.MusicFileUri.OriginalString)
                    , new XElement(nameof(entity.Duration), entity.Duration.GetMinuteAndSecondPart())
                    );
                    xmlRoot.Add(log);
                }
                xmlRoot.Save(_historyPath);
            }
        }

        public static IEnumerable<HistoryPlayItemEntity> GetHistoryPlayList()
        {
            var xmlRoot = GetLogRootElement();
            if (xmlRoot == null) return null;
            return xmlRoot.Elements().Select(e =>
            {
                var entity = new HistoryPlayItemEntity()
                {
                    MusicId = int.Parse(e.Element("MusicId").Value),
                    SingerId = int.Parse(e.Element("SingerId").Value),
                    AlbumId = int.Parse(e.Element("AlbumId").Value),
                    MusicName = e.Element("MusicName").Value,
                    SingerName = e.Element("SingerName").Value,
                    AlbumName = e.Element("AlbumName").Value,
                    MusicFileUri = new Uri(e.Element("MusicFileUri").Value, UriKind.Relative),
                };
                var duration = e.Element("Duration").Value;
                entity.Duration = TimeSpan.FromMinutes(double.Parse(duration.Substring(0, 2)))
                        .Add(TimeSpan.FromSeconds(double.Parse(duration.Substring(duration.Length - 2, 2))));
                return entity;
            }).Reverse();
        }

        /// <summary>
        /// 创建简单的xml并保存
        /// </summary>
        private static XDocument CreateXML()
        {
            var xml = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes")
                , new XElement(History_Root_Element));
            xml.Save(_historyPath);
            return xml;
        }

        /// <summary>
        /// 获取播放记录根元素
        /// </summary>
        /// <returns></returns>
        private static XElement GetLogRootElement()
        {
            try
            {
                var xml = XDocument.Load(_historyPath);
                var ele = xml.Element(History_Root_Element);
                return ele;
            }
            catch
            {
                return null;
            }
        }
    }
}
