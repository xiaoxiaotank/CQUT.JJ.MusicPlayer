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
    /// JmSlider.xaml 的交互逻辑
    /// </summary>
    public partial class JmSlider : Slider
    {
        private static readonly Type _ownerType = typeof(JmSlider);

        #region TrackBorderBrush Track边框画刷
        public Brush TrackBorderBrush
        {
            get { return (Brush)GetValue(TrackBorderBrushProperty); }
            set { SetValue(TrackBorderBrushProperty, value); }
        }

        public static readonly DependencyProperty TrackBorderBrushProperty =
            DependencyProperty.Register("TrackBorderBrush", typeof(Brush), _ownerType, new PropertyMetadata(new SolidColorBrush(Colors.White)));

        #endregion

        #region TrackBorderThickness Track边框粗细
        public Thickness TrackBorderThickness
        {
            get { return (Thickness)GetValue(TrackBorderThicknessProperty); }
            set { SetValue(TrackBorderThicknessProperty, value); }
        }

        public static readonly DependencyProperty TrackBorderThicknessProperty =
            DependencyProperty.Register("TrackBorderThickness", typeof(Thickness), _ownerType, new PropertyMetadata(new Thickness(1)));
        #endregion

        #region TrackBackground Track背景色
        public Brush TrackBackground
        {
            get { return (Brush)GetValue(TrackBackgroundProperty); }
            set { SetValue(TrackBackgroundProperty, value); }
        }

        public static readonly DependencyProperty TrackBackgroundProperty =
            DependencyProperty.Register("TrackBackground", typeof(Brush), _ownerType, new PropertyMetadata(new SolidColorBrush(Colors.Gray)));
        #endregion

        #region TrackHeight Track高度

        public double TrackHeight
        {
            get { return (double)GetValue(TrackHeightProperty); }
            set { SetValue(TrackHeightProperty, value); }
        }

        public static readonly DependencyProperty TrackHeightProperty =
            DependencyProperty.Register("TrackHeight", typeof(double), _ownerType, new PropertyMetadata(4d));
        #endregion

        #region TrackMargin Track外边距
        public Thickness TrackMargin
        {
            get { return (Thickness)GetValue(TrackMarginProperty); }
            set { SetValue(TrackMarginProperty, value); }
        }

        public static readonly DependencyProperty TrackMarginProperty =
            DependencyProperty.Register("TrackMargin", typeof(Thickness), _ownerType, new PropertyMetadata(new Thickness(5,0,5,0)));
        #endregion

        #region ThumbRadius Thumb半径
        public double ThumbRadius
        {
            get { return (double)GetValue(ThumbRadiusProperty); }
            set { SetValue(ThumbRadiusProperty, value); }
        }

        public static readonly DependencyProperty ThumbRadiusProperty =
            DependencyProperty.Register("ThumbRadius", typeof(double), _ownerType, new PropertyMetadata(10d));
        #endregion

        #region ThumbBackground Thumb背景色
        public Brush ThumbBackground
        {
            get { return (Brush)GetValue(ThumbBackgroundProperty); }
            set { SetValue(ThumbBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ThumbBackgroundProperty =
            DependencyProperty.Register("ThumbBackground", typeof(Brush), _ownerType, new PropertyMetadata(new SolidColorBrush(Colors.Red)));
        #endregion

        #region SelectionBrush 选择区域画刷
        public Brush SelectionBrush
        {
            get { return (Brush)GetValue(SelectionBrushProperty); }
            set { SetValue(SelectionBrushProperty, value); }
        }

        public static readonly DependencyProperty SelectionBrushProperty =
            DependencyProperty.Register("SelectionBrush", typeof(Brush), _ownerType, new PropertyMetadata(new SolidColorBrush(Colors.Yellow))); 
        #endregion







        static JmSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(_ownerType, new FrameworkPropertyMetadata(_ownerType));
        }
    }
}
