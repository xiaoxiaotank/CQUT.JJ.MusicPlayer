using System;
using System.Collections.Generic;
using System.Text;

namespace CQUT.JJ.MusicPlayer.EntityFramework.Persistences
{
    public class GlobalConstants
    {
        public const string MusicsRootPath = @"..\..\";

        public const string MusicsRootDirectoryName = "MusicLibrary";

        public const string LogRootPath = @"..\..\";

        public const string LogRootDirectoryName = "Log";

        /// <summary>
        /// 音乐词典路径
        /// </summary>
        public const string MusicDictionaryPath = @"Configs/Files/MusicDictionary.bat";
        /// <summary>
        /// 音乐停用词典路径
        /// </summary>
        public const string MusicStopDictionaryPath = @"Configs/Files/MusicStopDictionary.bat";

        public static readonly string[] Music_File_Suffix = { ".mp3" };

        
    }
}
