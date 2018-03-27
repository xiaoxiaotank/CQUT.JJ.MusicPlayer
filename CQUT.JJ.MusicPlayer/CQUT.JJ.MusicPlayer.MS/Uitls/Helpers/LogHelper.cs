using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
using CQUT.JJ.MusicPlayer.EntityFramework.Persistences;
using CQUT.JJ.MusicPlayer.EntityFramework.Persistences.Extensions;
using CQUT.JJ.MusicPlayer.MS.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CQUT.JJ.MusicPlayer.MS.Uitls.Helpers
{
    public static class LogHelper
    {
        private const string Log_Root_Element = "Log";

        private const string Log_Item_Element = "LogItem";

        private const string Log_FullDirectory = GlobalConstants.LogRootPath + GlobalConstants.LogRootDirectoryName;

        private static readonly string _logPath = Path.Combine(Log_FullDirectory, "Log.xml");

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="logEntity"></param>
        public static void Log(LogItemEntity logEntity)
        {
            XElement xmlRoot = null;
            Directory.CreateDirectory(Log_FullDirectory);
            if (!File.Exists(_logPath))
                xmlRoot = CreateXML().Element(Log_Root_Element);
            if(xmlRoot == null)
                xmlRoot = GetLogRootElement();

            var log = new XElement(Log_Item_Element,
                new XElement(nameof(logEntity.Message), logEntity.Message)
                , new XElement(nameof(logEntity.DateTime), logEntity.DateTime.ToStandardDateTimeOfChina())
                , new XElement(nameof(logEntity.UserName), logEntity.UserName)
                , new XElement(nameof(logEntity.Source), logEntity.Source)
                , new XElement(nameof(logEntity.Type), Enum.GetName(typeof(LogType),logEntity.Type))
                );
            xmlRoot.Add(log);
            xmlRoot.Save(_logPath);
        }

        public static IEnumerable<LogItemEntity> GetLogs()
        {
            var xmlRoot = GetLogRootElement();
            return xmlRoot.Elements().Select(e => new LogItemEntity()
            {
                Message = e.Element("Message").Value,
                Type = (LogType)Enum.Parse(typeof(LogType), e.Element("Type").Value),
                UserName = e.Element("UserName").Value,
                Source = e.Element("Source").Value,
                DateTime = DateTime.Parse(e.Element("DateTime").Value)
            });
        }

        /// <summary>
        /// 创建简单的xml并保存
        /// </summary>
        private static XDocument CreateXML()
        {
            var xml = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes")
                ,new XElement(Log_Root_Element));
            xml.Save(_logPath);
            return xml;
        }

        /// <summary>
        /// 获取日志记录根元素
        /// </summary>
        /// <returns></returns>
        private static XElement GetLogRootElement()
        {
            var xml = XDocument.Load(_logPath);
            var ele = xml.Element(Log_Root_Element);
            return ele;
        }
    }
}
