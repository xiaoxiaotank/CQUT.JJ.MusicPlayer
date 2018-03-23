using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace CQUT.JJ.MusicPlayer.EntityFramework.Persistences.APIs
{
    public class APIClass
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        private static string ShortPathName = "";

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        private static string durLength = "";

        [DllImport("winmm.dll", EntryPoint = "mciSendString", CharSet = CharSet.Auto)]
        private static extern int mciSendString(
            string lpstrCommand,
            string lpstrReturnString,
            int uReturnLength,
            int hwndCallback
        );
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetShortPathName(
            string lpszLongPath,
            string shortFile,
            int cchBuffer
        );

        private static string GetCurrentPath(string name)
        {
            if (name.Length < 1) return string.Empty;
            name = name.Trim();
            name = name.Substring(0, name.Length - 1);
            return name;
        }

        public static string GetMusicDurationString(string fileName)
        {
            durLength = string.Empty.PadLeft(128, ' ');
            ShortPathName = string.Empty.PadLeft(260, ' ');

            GetShortPathName(fileName, ShortPathName, ShortPathName.Length);
            ShortPathName = GetCurrentPath(ShortPathName);

            var cmdStr = "status  " + ShortPathName + "   length";
            mciSendString(cmdStr, durLength, durLength.Length, 0);

            durLength = durLength.Trim();
            return durLength;
        }

    }
}
