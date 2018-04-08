using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CQUT.JJ.MusicPlayer.Controls.Controls
{
    /// <summary>
    /// JmMenuItem.xaml 的交互逻辑
    /// </summary>
    public partial class JmMenuItem : MenuItem
    {
        private readonly static Type _ownerType = typeof(JmMenuItem);


        #region 鼠标悬浮时背景色

        public Brush HighlightedBackground
        {
            get { return (Brush)GetValue(HighlightedBackgroundProperty); }
            set { SetValue(HighlightedBackgroundProperty, value); }
        }

        public static readonly DependencyProperty HighlightedBackgroundProperty =
            DependencyProperty.Register(nameof(HighlightedBackground), typeof(Brush), _ownerType, new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F1F1F1")))); 

        #endregion




        static JmMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(_ownerType, new FrameworkPropertyMetadata(_ownerType));
        }
    }
}
