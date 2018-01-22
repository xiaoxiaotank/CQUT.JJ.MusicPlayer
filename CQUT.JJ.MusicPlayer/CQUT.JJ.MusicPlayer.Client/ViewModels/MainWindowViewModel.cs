using CQUT.JJ.MusicPlayer.Client.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CQUT.JJ.MusicPlayer.Client.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private Brush _topFloorBackground = new SolidColorBrush(Colors.White);
        private double _topBarBackgroundOpacity  = 1;
        private double _leftBarBackgroundOpacity = 1;
        private double _bottomBarBackgroundOpacity  = 1;
        private double _contentBackgroundOpacity  = 1;
        private double _backgroundOpacity  = 1;

        public event PropertyChangedEventHandler PropertyChanged;

        public Brush TopFloorBackground
        {
            get { return _topFloorBackground; }
            set
            {
                _topFloorBackground = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TopFloorBackground)));
            }
        }

        public double TopBarBackgroundOpacity
        {
            get { return _topBarBackgroundOpacity; }
            set
            {
                _topBarBackgroundOpacity = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TopBarBackgroundOpacity)));
            }
        }
        public double LeftBarBackgroundOpacity
        {
            get { return _leftBarBackgroundOpacity; }
            set
            {
                _leftBarBackgroundOpacity = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LeftBarBackgroundOpacity)));
            }
        }
        public double BottomBarBackgroundOpacity
        {
            get { return _bottomBarBackgroundOpacity; }
            set
            {
                _bottomBarBackgroundOpacity = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BottomBarBackgroundOpacity)));
            }
        }
        public double ContentBackgroundOpacity
        {
            get { return _contentBackgroundOpacity; }
            set
            {
                _contentBackgroundOpacity = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ContentBackgroundOpacity)));
            }
        }
        public double BackgroundOpacity
        {
            get { return _backgroundOpacity; }
            set
            {
                _backgroundOpacity = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackgroundOpacity)));
            }
        }

    }
}
