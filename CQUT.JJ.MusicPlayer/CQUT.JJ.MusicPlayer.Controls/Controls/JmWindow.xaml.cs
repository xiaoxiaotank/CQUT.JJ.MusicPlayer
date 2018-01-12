using CQUT.JJ.MusicPlayer.Controls.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CQUT.JJ.MusicPlayer.Controls.Controls
{
    /// <summary>
    /// JmWindow.xaml 的交互逻辑
    /// </summary>
    public partial class JmWindow : Window
    {
        private static Type _ownerType = typeof(JmWindow);

        #region TopBarHeight 顶栏高度

        public double TopBarHeight
        {
            get { return (double)GetValue(TopBarHeightProperty); }
            set { SetValue(TopBarHeightProperty, value); }
        }

        public static readonly DependencyProperty TopBarHeightProperty =
            DependencyProperty.Register(nameof(TopBarHeight), typeof(double), _ownerType, new PropertyMetadata(50d));
        #endregion

        #region TopBarBackground 顶栏背景
        public Brush TopBarBackground
        {
            get { return (Brush)GetValue(TopBarBackgroundProperty); }
            set { SetValue(TopBarBackgroundProperty, value); }
        }

        public static readonly DependencyProperty TopBarBackgroundProperty = 
            DependencyProperty.Register(nameof(TopBarBackground), typeof(Brush), _ownerType, new PropertyMetadata(new SolidColorBrush(Colors.White)));

        #endregion

        #region TopBarContent 顶栏内容
        public object TopBarContent
        {
            get { return (object)GetValue(TopBarContentProperty); }
            set { SetValue(TopBarContentProperty, value); }
        }

        public static readonly DependencyProperty TopBarContentProperty =
            DependencyProperty.Register("TopBarContent", typeof(object), _ownerType, new PropertyMetadata(null));
        #endregion

        #region LeftBarWidth 左侧栏宽度

        public double LeftBarWidth
        {
            get { return (double)GetValue(LeftBarWidthProperty); }
            set { SetValue(LeftBarWidthProperty, value); }
        }

        public static readonly DependencyProperty LeftBarWidthProperty =
            DependencyProperty.Register(nameof(LeftBarWidth), typeof(double), _ownerType, new PropertyMetadata(double.NaN));
        #endregion

        #region LeftBarBackground 左侧栏背景
        public Brush LeftBarBackground
        {
            get { return (Brush)GetValue(LeftBarBackgroundProperty); }
            set { SetValue(LeftBarBackgroundProperty, value); }
        }

        public static readonly DependencyProperty LeftBarBackgroundProperty =
            DependencyProperty.Register(nameof(LeftBarBackground), typeof(Brush), _ownerType, new PropertyMetadata(new SolidColorBrush(Colors.White)));

        #endregion

        #region LeftBarContent 左侧栏内容
        public object LeftBarContent
        {
            get { return (object)GetValue(LeftBarContentProperty); }
            set { SetValue(LeftBarContentProperty, value); }
        }

        public static readonly DependencyProperty LeftBarContentProperty =
            DependencyProperty.Register("LeftBarContent", typeof(object), _ownerType, new PropertyMetadata(null));
        #endregion

        #region BottomBarHeight 底侧栏高度

        public double BottomBarHeight
        {
            get { return (double)GetValue(BottomBarHeightProperty); }
            set { SetValue(BottomBarHeightProperty, value); }
        }

        public static readonly DependencyProperty BottomBarHeightProperty =
            DependencyProperty.Register(nameof(BottomBarHeight), typeof(double), _ownerType, new PropertyMetadata(double.NaN));
        #endregion

        #region BottomBarBackground 底侧栏背景
        public Brush BottomBarBackground
        {
            get { return (Brush)GetValue(BottomBarBackgroundProperty); }
            set { SetValue(BottomBarBackgroundProperty, value); }
        }

        public static readonly DependencyProperty BottomBarBackgroundProperty =
            DependencyProperty.Register(nameof(BottomBarBackground), typeof(Brush), _ownerType, new PropertyMetadata(new SolidColorBrush(Colors.White)));

        #endregion

        #region BottomBarContent 底侧栏内容
        public object BottomBarContent
        {
            get { return (object)GetValue(BottomBarContentProperty); }
            set { SetValue(BottomBarContentProperty, value); }
        }

        public static readonly DependencyProperty BottomBarContentProperty =
            DependencyProperty.Register("BottomBarContent", typeof(object), _ownerType, new PropertyMetadata(null));
        #endregion

        #region ToolBarMenuItemSource 工具栏菜单源
        public IEnumerable ToolBarMenuItemSource
        {
            get { return (IEnumerable)GetValue(ToolBarMenuItemSourceProperty); }
            set { SetValue(ToolBarMenuItemSourceProperty, value); }
        }

        public static readonly DependencyProperty ToolBarMenuItemSourceProperty =
            DependencyProperty.Register("ToolBarMenuItemSource", typeof(IEnumerable), _ownerType, new PropertyMetadata(null));
        #endregion

        #region ToolBarMenuItems 暴露给用户的菜单集合
        private ObservableCollection<JmTransparentButton> _toolBarMenuItems = new ObservableCollection<JmTransparentButton>();

        public ObservableCollection<JmTransparentButton> ToolBarMenuItems
        {
            get { return _toolBarMenuItems; }
        }
        #endregion

        #region IsOpenAreo 是否开启Areo效果
        public bool IsOpenAreo
        {
            get { return (bool)GetValue(IsOpenAreoProperty); }
            set { SetValue(IsOpenAreoProperty, value); }
        }
        public static readonly DependencyProperty IsOpenAreoProperty =
            DependencyProperty.Register("IsOpenAreo", typeof(bool), _ownerType, new PropertyMetadata(false, IsOpenAreoPropertyChanged));

        private static void IsOpenAreoPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue && (bool)e.NewValue == true)
            {
                var window = d as Window;
                window.OpenAero();
                window.Background = new SolidColorBrush(Colors.Transparent);
            }
                
        }
        #endregion

        #region BackgroundOpacity 背景透明度
        public double BackgroundOpacity
        {
            get { return (double)GetValue(BackgroundOpacityProperty); }
            set { SetValue(BackgroundOpacityProperty, value); }
        }

        public static readonly DependencyProperty BackgroundOpacityProperty =
            DependencyProperty.Register("BackgroundOpacity", typeof(double), _ownerType, new PropertyMetadata(1d));
        #endregion

        #region ContentBackground 内容背景
        public Brush ContentBackground
        {
            get { return (Brush)GetValue(ContentBackgroundProperty); }
            set { SetValue(ContentBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ContentBackgroundProperty =
            DependencyProperty.Register("ContentBackground", typeof(Brush), _ownerType, new PropertyMetadata(new SolidColorBrush(Colors.White)));

        #endregion


        static JmWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(_ownerType, new FrameworkPropertyMetadata(_ownerType));
        }

        public JmWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            if(IsOpenAreo)
                this.OpenAero();
        }
    }  
}
