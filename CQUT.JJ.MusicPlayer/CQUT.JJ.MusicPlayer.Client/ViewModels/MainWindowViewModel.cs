using CQUT.JJ.MusicPlayer.Client.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CQUT.JJ.MusicPlayer.Client.ViewModels
{
    public class MainWindowViewModel
    {
        public Brush TopFloorBackground { get; set; } = new SolidColorBrush(Colors.White);

        public double TopBarBackgroundOpacity { get; set; } = 1;
        public double LeftBarBackgroundOpacity { get; set; } = 1;
        public double BottomBarBackgroundOpacity { get; set; } = 1;
        public double ContentBackgroundOpacity { get; set; } = 1;
        public double BackgroundOpacity { get; set; } = 1;


    }
}
