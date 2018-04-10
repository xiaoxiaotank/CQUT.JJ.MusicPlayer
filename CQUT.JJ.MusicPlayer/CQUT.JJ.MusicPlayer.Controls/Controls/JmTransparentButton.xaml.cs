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
    /// TransparentButton.xaml 的交互逻辑
    /// </summary>
    public partial class JmTransparentButton : Button
    {
        private static Type _ownerType = typeof(JmTransparentButton);

        #region CornerRadius 圆角半径
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), _ownerType, new PropertyMetadata(new CornerRadius(0)));
        #endregion

        #region 鼠标悬浮时背景色

        public Brush HighlightedBackground
        {
            get { return (Brush)GetValue(HighlightedBackgroundProperty); }
            set { SetValue(HighlightedBackgroundProperty, value); }
        }

        public static readonly DependencyProperty HighlightedBackgroundProperty =
            DependencyProperty.Register(nameof(HighlightedBackground), typeof(Brush), _ownerType, new PropertyMetadata(new SolidColorBrush(Colors.SkyBlue)));

        #endregion


        static JmTransparentButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(_ownerType, new FrameworkPropertyMetadata(_ownerType));
        }
    }
}
