using CQUT.JJ.MusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Client.Utils.EventUtils
{
    public static class MusicSearchInfoChangedUtil
    {
        public static event EventHandler<QMSearchChangedArgs> QMSearchChangedEvent;

        public static void InvokeFromQM(IEnumerable<QMSongInfoModel> qmSongInfoModels)
        {
            var e = new QMSearchChangedArgs(qmSongInfoModels);
            QMSearchChangedEvent(null, e);
        }
    }

    public class QMSearchChangedArgs : EventArgs
    {
        public IEnumerable<QMSongInfoModel> QMSongInfoModels { get; set; }
        public QMSearchChangedArgs(IEnumerable<QMSongInfoModel> qmSongInfoModels)
        {
            QMSongInfoModels = qmSongInfoModels;
        }
    }
}
