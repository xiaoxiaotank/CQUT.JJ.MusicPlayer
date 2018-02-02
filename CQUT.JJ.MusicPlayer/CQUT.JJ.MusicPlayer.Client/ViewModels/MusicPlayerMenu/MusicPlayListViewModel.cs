using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.Client.ViewModels.MusicPlayerMenu
{
    public class MusicPlayListViewModel : INotifyPropertyChanged
    {
        private int _musicTotalCount = 0;

        private ObservableCollection<MusicOfPlayListViewModel> _musicPlayListItems = new ObservableCollection<MusicOfPlayListViewModel>();

        public ObservableCollection<MusicOfPlayListViewModel> MusicPlayList
        {
            get { return _musicPlayListItems; }
        }


        public int MusicTotalCount
        {
            get { return _musicTotalCount; }
            private set
            {
                _musicTotalCount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MusicTotalCount)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MusicPlayListViewModel()
        {
            _musicPlayListItems.CollectionChanged += _musicPlayListItems_CollectionChanged;
        }

        private void _musicPlayListItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    MusicTotalCount++;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    MusicTotalCount--;
                    break;
            }
        }
    }

    public class MusicOfPlayListViewModel : INotifyPropertyChanged
    {
        private string _id;
        private string _name;
        private string _singerName;
        private string _timeDuration;

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

        public string TimeDuration
        {
            get { return _timeDuration; }
            set
            {
                _timeDuration = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TimeDuration)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
