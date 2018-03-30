using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Client.ViewModels
{
    public class MusicInfoViewModel : INotifyPropertyChanged
    {
        private bool _isActivated = false;

        public bool IsActivated
        {
            get { return _isActivated; }
            set
            {
                _isActivated = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsActivated)));
            }
        }

        public int Id { get; set; }

        public int SingerId { get; set; }

        public int AlbumId { get; set; }

        public string MusicName { get; set; }

        public string SingerName { get; set; }

        public string AlbumName { get; set; }

        public TimeSpan Duration { get; set; }

        public string DurationDescription { get; set; }

        public string FileUrl { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
