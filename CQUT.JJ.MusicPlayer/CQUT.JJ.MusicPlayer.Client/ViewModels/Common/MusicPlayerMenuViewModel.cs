using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Client.ViewModels
{
    public class MusicPlayerMenuViewModel : INotifyPropertyChanged
    {
        private string _musicName = string.Empty;
        private string _singerName = string.Empty;
        private Uri _photoUri;

        public int Id { get; set; }

        public string MusicName
        {
            get { return _musicName; }
            set
            {
                _musicName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MusicName)));
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

        public Uri PhotoUri
        {
            get { return _photoUri; }
            set
            {
                _photoUri = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PhotoUri)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
