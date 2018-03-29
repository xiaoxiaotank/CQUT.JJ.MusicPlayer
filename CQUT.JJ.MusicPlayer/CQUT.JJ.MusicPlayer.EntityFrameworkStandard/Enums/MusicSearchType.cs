using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CQUT.JJ.MusicPlayer.EntityFramework.Enums
{
    public enum MusicSearchType
    {
        [Description("单曲")]
        Song,
        [Description("专辑")]
        Album,
        [Description("MV")]
        MV,
        [Description("歌单")]
        PlayList,
        [Description("用户")]
        User,
        [Description("歌词")]
        Lyric,
        [Description("歌手")]
        Singer
    }
}
