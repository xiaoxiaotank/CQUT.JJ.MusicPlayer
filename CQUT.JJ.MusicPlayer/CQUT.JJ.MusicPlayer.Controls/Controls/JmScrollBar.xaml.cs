using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// JmScrollBar.xaml 的交互逻辑
    /// </summary>
    public partial class JmScrollBar : ScrollBar
    {
        private static readonly Type _ownerType = typeof(JmScrollBar);

        #region UpArrowVisibility 向上的箭头可见性
        public Visibility UpArrowVisibility
        {
            get { return (Visibility)GetValue(UpArrowVisibilityProperty); }
            set { SetValue(UpArrowVisibilityProperty, value); }
        }

        public static readonly DependencyProperty UpArrowVisibilityProperty =
            DependencyProperty.Register("UpArrowVisibility", typeof(Visibility), _ownerType, new PropertyMetadata(Visibility.Visible));
        #endregion

        #region DownArrowVisibility 向下的箭头可见性
        public Visibility DownArrowVisibility
        {
            get { return (Visibility)GetValue(DownArrowVisibilityProperty); }
            set { SetValue(DownArrowVisibilityProperty, value); }
        }

        public static readonly DependencyProperty DownArrowVisibilityProperty =
            DependencyProperty.Register("DownArrowVisibility", typeof(Visibility), _ownerType, new PropertyMetadata(Visibility.Visible));
        #endregion

        #region ThumbWidth Thumb宽度
        public double ThumbWidth
        {
            get { return (double)GetValue(ThumbWidthProperty); }
            set { SetValue(ThumbWidthProperty, value); }
        }

        public static readonly DependencyProperty ThumbWidthProperty =
            DependencyProperty.Register("ThumbWidth", typeof(double), _ownerType, new PropertyMetadata(10d));
        #endregion

        #region ThumbHeight Thumb高度
        public double ThumbHeight
        {
            get { return (double)GetValue(ThumbHeightProperty); }
            set { SetValue(ThumbHeightProperty, value); }
        }

        public static readonly DependencyProperty ThumbHeightProperty =
            DependencyProperty.Register("ThumbHeight", typeof(double), _ownerType, new PropertyMetadata(10d));
        #endregion

        #region ThumbCornerRadius Thumb圆角半径
        public CornerRadius ThumbCornerRadius
        {
            get { return (CornerRadius)GetValue(ThumbCornerRadiusProperty); }
            set { SetValue(ThumbCornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty ThumbCornerRadiusProperty =
            DependencyProperty.Register("ThumbCornerRadius", typeof(CornerRadius), _ownerType, new PropertyMetadata(new CornerRadius()));
        #endregion

        #region ThumbBackground Thumb背景色
        public Brush ThumbBackground
        {
            get { return (Brush)GetValue(ThumbBackgroundProperty); }
            set { SetValue(ThumbBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ThumbBackgroundProperty =
            DependencyProperty.Register("ThumbBackground", typeof(Brush), _ownerType, new PropertyMetadata(new SolidColorBrush(Colors.White)));
        #endregion

        #region ThumbVisibility Thumb可见性
        public Visibility ThumbVisibility
        {
            get { return (Visibility)GetValue(ThumbVisibilityProperty); }
            set { SetValue(ThumbVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ThumbVisibilityProperty =
            DependencyProperty.Register("ThumbVisibility", typeof(Visibility), _ownerType, new PropertyMetadata(Visibility.Visible));
        #endregion

        #region ThumbMouseOverBackground Thumb鼠标悬浮时背景颜色
        public Brush ThumbMouseOverBackground
        {
            get { return (Brush)GetValue(ThumbMouseOverBackgroundProperty); }
            set { SetValue(ThumbMouseOverBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ThumbMouseOverBackgroundProperty =
            DependencyProperty.Register("ThumbMouseOverBackground", typeof(Brush), _ownerType, new PropertyMetadata(new SolidColorBrush(Colors.Wheat)));
        #endregion


        static JmScrollBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(_ownerType, new FrameworkPropertyMetadata(_ownerType));
        }

        public JmScrollBar()
        {
            if (ThumbWidth > Width)
                ThumbWidth = Width;
        }
    }
}
