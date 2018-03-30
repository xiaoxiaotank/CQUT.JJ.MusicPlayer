using CQUT.JJ.MusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Client.ViewModels
{
    public class QMInfoViewModel : INotifyPropertyChanged
    {
        private string _id = string.Empty;
        private string _name = string.Empty;
        private string _singerName = string.Empty;
        private string _albumName = string.Empty;
        private string _timeDuration = string.Empty;
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

        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Id)));
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        public string SingerName
        {
            get { return _singerName; }
            set
            {
                _singerName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SingerName)));
            }
        }

        public string AlbumName
        {
            get { return _albumName; }
            set
            {
                _albumName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AlbumName)));
            }
        }

        public string TimeDuration
        {
            get { return _timeDuration; }
            set
            {
                _timeDuration = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TimeDuration)));
            }
        }

        public string SourcePath { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
