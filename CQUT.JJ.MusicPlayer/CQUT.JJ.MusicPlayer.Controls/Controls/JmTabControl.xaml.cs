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
    /// JmTabControl.xaml 的交互逻辑
    /// </summary>
    public partial class JmTabControl : TabControl
    {
        private static readonly Type _ownerType = typeof(JmTabControl);

        #region HeaderPanelHeight 选项卡高度
        public double HeaderPanelHeight
        {
            get { return (double)GetValue(HeaderPanelHeightProperty); }
            set { SetValue(HeaderPanelHeightProperty, value); }
        }

        public static readonly DependencyProperty HeaderPanelHeightProperty =
            DependencyProperty.Register("HeaderPanelHeight", typeof(double), _ownerType, new PropertyMetadata(0d)); 
        #endregion



        #region HeaderPanelHorizontalAlignment 选项卡水平对齐方式
        public HorizontalAlignment HeaderPanelHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(HeaderPanelHorizontalAlignmentProperty); }
            set { SetValue(HeaderPanelHorizontalAlignmentProperty, value); }
        }

        public static readonly DependencyProperty HeaderPanelHorizontalAlignmentProperty =
            DependencyProperty.Register("HeaderPanelHorizontalAlignment", typeof(HorizontalAlignment), _ownerType, new PropertyMetadata(HorizontalAlignment.Left));
        #endregion

        #region HeaderPanelVerticalAlignment 选项卡垂直对齐方式
        public VerticalAlignment HeaderPanelVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(HeaderPanelVerticalAlignmentProperty); }
            set { SetValue(HeaderPanelVerticalAlignmentProperty, value); }
        }

        public static readonly DependencyProperty HeaderPanelVerticalAlignmentProperty =
            DependencyProperty.Register("HeaderPanelVerticalAlignment", typeof(VerticalAlignment), _ownerType, new PropertyMetadata(VerticalAlignment.Center));
        #endregion

        #region HeaderPanelBackground 选项卡背景色
        public Brush HeaderPanelBackground
        {
            get { return (Brush)GetValue(HeaderPanelBackgroundProperty); }
            set { SetValue(HeaderPanelBackgroundProperty, value); }
        }

        public static readonly DependencyProperty HeaderPanelBackgroundProperty =
            DependencyProperty.Register("HeaderPanelBackground", typeof(Brush), _ownerType, new PropertyMetadata(new SolidColorBrush(Colors.White)));
        #endregion

        #region HeaderPanelBackgroundOpacity 选项卡背景色透明度
        public double HeaderPanelBackgroundOpacity
        {
            get { return (double)GetValue(HeaderPanelBackgroundOpacityProperty); }
            set { SetValue(HeaderPanelBackgroundOpacityProperty, value); }
        }

        public static readonly DependencyProperty HeaderPanelBackgroundOpacityProperty =
            DependencyProperty.Register("HeaderPanelBackgroundOpacity", typeof(double), _ownerType, new PropertyMetadata(1d)); 
        #endregion

        #region IsShowContentPage 是否展示内容页
        public bool IsShowContentPage
        {
            get { return (bool)GetValue(IsShowContentPageProperty); }
            set { SetValue(IsShowContentPageProperty, value); }
        }

        public static readonly DependencyProperty IsShowContentPageProperty =
            DependencyProperty.Register("IsShowContentPage", typeof(bool), _ownerType, new PropertyMetadata(true));
        #endregion


        static JmTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(_ownerType, new FrameworkPropertyMetadata(_ownerType));
        }
    }
}
