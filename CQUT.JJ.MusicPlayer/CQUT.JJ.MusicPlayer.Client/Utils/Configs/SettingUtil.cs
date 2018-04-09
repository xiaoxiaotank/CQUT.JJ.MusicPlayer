using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CQUT.JJ.MusicPlayer.Client.Utils.Configs
{
    public static class SettingUtil
    {
        private const string Setting_Root_Element = "Setting";

        private const string Setting_Item_Element = "SettingItem";

        private static readonly string _settingFullDirectory = Path.Combine(Environment.CurrentDirectory,"Configs");

        private static readonly string _settingPath = Path.Combine(_settingFullDirectory, "Setting.xml");

        private static readonly Object _obj = new object();

        /// <summary>
        /// 设置下载路径
        /// </summary>
        /// <param name="path"></param>
        public static void SetDownloadPath(string path)
        {
            XElement xmlRoot = null;
            lock (_obj)
            {
                if (!File.Exists(_settingPath))
                    xmlRoot = CreateDefaultSettingXML().Element(Setting_Root_Element);
                else if (xmlRoot == null)
                    xmlRoot = GetSettingRootElement();

                var downloadElement = CreateXElement(xmlRoot, nameof(SettingEntity.Download));
                var downloadPathElement = CreateXElement(downloadElement, nameof(Download.DownloadPath));

                downloadElement.Value = path;

                xmlRoot.Save(_settingPath);
            }
        }

        /// <summary>
        /// 获取下载路径
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetDowloadPathAsync()
        {
            var task = Task.Factory.StartNew(() =>
            {
                XElement xmlRoot = null;
                if (!File.Exists(_settingPath))
                    xmlRoot = CreateDefaultSettingXML().Element(Setting_Root_Element);
                else if (xmlRoot == null)
                    xmlRoot = GetSettingRootElement();

                var downloadElement = CreateXElement(xmlRoot, nameof(SettingEntity.Download));
                var downloadPathElement = CreateXElement(downloadElement, nameof(Download.DownloadPath));

                if(downloadElement.Value == null)
                {
                    downloadElement.Value = SettingConstants.Default_Dowload_Path;
                }
                return downloadElement.Value;
            });
            var result = await task;
            return result;
        }

        /// <summary>
        /// 创建特定元素，除非它已经存在
        /// </summary>
        /// <param name="elementName"></param>
        /// <returns></returns>
        private static XElement CreateXElement(XElement parent, string elementName)
        {
            var element = parent.Element(elementName);
            if (element == null)
            {
                element = new XElement(elementName);
                parent.Add(element);
            }

            return element;
        }

        /// <summary>
        /// 创建默认的设置文档并保存
        /// </summary>
        private static XDocument CreateDefaultSettingXML()
        {
            Directory.CreateDirectory(_settingFullDirectory);
            var xml = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes")
                , new XElement(Setting_Root_Element,
                    new XElement(nameof(SettingEntity.Download)
                    , new XElement(nameof(Download.DownloadPath),SettingConstants.Default_Dowload_Path)
                    )
                )
            );
            xml.Save(_settingPath);
            return xml;
        }

        /// <summary>
        /// 获取设置文档根元素
        /// </summary>
        /// <returns></returns>
        private static XElement GetSettingRootElement()
        {
            var xml = XDocument.Load(_settingPath);
            var ele = xml.Element(Setting_Root_Element);
            return ele;
        }



        class SettingEntity
        {
            public static Normal Normal;

            public static Download Download;
        }

        class Normal
        {

        }

        class Download
        {
            public static string DownloadPath;
        }
    }
}
