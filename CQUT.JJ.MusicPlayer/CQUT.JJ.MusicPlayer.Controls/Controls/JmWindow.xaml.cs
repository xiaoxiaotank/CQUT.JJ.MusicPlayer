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
using System.Windows.Forms;
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

        public NotifyIcon NotifyIcon { get; set; }

        #region TopBarHeadHeight 顶栏头部高度
        public double TopBarHeadHeight
        {
            get { return (double)GetValue(TopBarHeadHeightProperty); }
            set { SetValue(TopBarHeadHeightProperty, value); }
        }

        public static readonly DependencyProperty TopBarHeadHeightProperty =
            DependencyProperty.Register("TopBarHeadHeight", typeof(double), _ownerType, new PropertyMetadata(0d)); 
        #endregion

        #region TopBarHeight 顶栏高度

        public double TopBarHeight
        {
            get { return (double)GetValue(TopBarHeightProperty); }
            set { SetValue(TopBarHeightProperty, value); }
        }

        public static readonly DependencyProperty TopBarHeightProperty =
            DependencyProperty.Register(nameof(TopBarHeight), typeof(double), _ownerType, new PropertyMetadata(50d));
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

        #region TopBarHeadBackgroundOpacity 顶栏头部透明度
        public double TopBarHeadBackgroundOpacity
        {
            get { return (double)GetValue(TopBarHeadBackgroundOpacityProperty); }
            set { SetValue(TopBarHeadBackgroundOpacityProperty, value); }
        }

        public static readonly DependencyProperty TopBarHeadBackgroundOpacityProperty =
            DependencyProperty.Register("TopBarHeadBackgroundOpacity", typeof(double), _ownerType, new PropertyMetadata(1d));
        #endregion

        #region TopBarBackgroundOpacity TopBar背景透明度
        public double TopBarBackgroundOpacity
        {
            get { return (double)GetValue(TopBarBackgroundOpacityProperty); }
            set { SetValue(TopBarBackgroundOpacityProperty, value); }
        }

        public static readonly DependencyProperty TopBarBackgroundOpacityProperty =
            DependencyProperty.Register("TopBarBackgroundOpacity", typeof(double), _ownerType, new PropertyMetadata(1d));
        #endregion

        #region LeftBarBackgroundOpacity LeftBar背景透明度
        public double LeftBarBackgroundOpacity
        {
            get { return (double)GetValue(LeftBarBackgroundOpacityProperty); }
            set { SetValue(LeftBarBackgroundOpacityProperty, value); }
        }

        public static readonly DependencyProperty LeftBarBackgroundOpacityProperty =
            DependencyProperty.Register("LeftBarBackgroundOpacity", typeof(double), _ownerType, new PropertyMetadata(1d));
        #endregion

        #region BottomBarBackgroundOpacity BottomBar背景透明度
        public double BottomBarBackgroundOpacity
        {
            get { return (double)GetValue(BottomBarBackgroundOpacityProperty); }
            set { SetValue(BottomBarBackgroundOpacityProperty, value); }
        }

        public static readonly DependencyProperty BottomBarBackgroundOpacityProperty =
            DependencyProperty.Register("BottomBarBackgroundOpacity", typeof(double), _ownerType, new PropertyMetadata(1d));
        #endregion

        #region ContentBackgroundOpacity Content背景透明度
        public double ContentBackgroundOpacity
        {
            get { return (double)GetValue(ContentBackgroundOpacityProperty); }
            set { SetValue(ContentBackgroundOpacityProperty, value); }
        }

        public static readonly DependencyProperty ContentBackgroundOpacityProperty =
            DependencyProperty.Register("ContentBackgroundOpacity", typeof(double), _ownerType, new PropertyMetadata(1d));
        #endregion

        #region TopBarHeadBackground 顶栏头部背景色
        public Brush TopBarHeadBackground
        {
            get { return (Brush)GetValue(TopBarHeadBackgroundProperty); }
            set { SetValue(TopBarHeadBackgroundProperty, value); }
        }

        public static readonly DependencyProperty TopBarHeadBackgroundProperty =
            DependencyProperty.Register("TopBarHeadBackground", typeof(Brush), _ownerType, new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));
        #endregion

        #region TopBarBackground 顶栏背景
        public Brush TopBarBackground
        {
            get { return (Brush)GetValue(TopBarBackgroundProperty); }
            set { SetValue(TopBarBackgroundProperty, value); }
        }

        public static readonly DependencyProperty TopBarBackgroundProperty =
            DependencyProperty.Register(nameof(TopBarBackground), typeof(Brush), _ownerType, new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        #endregion

        #region LeftBarBackground 左侧栏背景
        public Brush LeftBarBackground
        {
            get { return (Brush)GetValue(LeftBarBackgroundProperty); }
            set { SetValue(LeftBarBackgroundProperty, value); }
        }

        public static readonly DependencyProperty LeftBarBackgroundProperty =
            DependencyProperty.Register(nameof(LeftBarBackground), typeof(Brush), _ownerType, new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        #endregion

        #region BottomBarBackground 底侧栏背景
        public Brush BottomBarBackground
        {
            get { return (Brush)GetValue(BottomBarBackgroundProperty); }
            set { SetValue(BottomBarBackgroundProperty, value); }
        }

        public static readonly DependencyProperty BottomBarBackgroundProperty =
            DependencyProperty.Register(nameof(BottomBarBackground), typeof(Brush), _ownerType, new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        #endregion

        #region ContentBackground 内容背景
        public Brush ContentBackground
        {
            get { return (Brush)GetValue(ContentBackgroundProperty); }
            set { SetValue(ContentBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ContentBackgroundProperty =
            DependencyProperty.Register("ContentBackground", typeof(Brush), _ownerType, new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        #endregion

        #region MinimizedVisibility 最小化按钮可见性
        public Visibility MinimizedVisibility
        {
            get { return (Visibility)GetValue(MinimizedVisibilityProperty); }
            set { SetValue(MinimizedVisibilityProperty, value); }
        }

        public static readonly DependencyProperty MinimizedVisibilityProperty =
            DependencyProperty.Register("MinimizedVisibility", typeof(Visibility), _ownerType, new PropertyMetadata(Visibility.Visible));
        #endregion

        #region MaximizedOrNormalVisibility 最大化或还原按钮可见性
        public Visibility MaximizedOrNormalVisibility
        {
            get { return (Visibility)GetValue(MaximizedOrNormalVisibilityProperty); }
            set { SetValue(MaximizedOrNormalVisibilityProperty, value); }
        }

        public static readonly DependencyProperty MaximizedOrNormalVisibilityProperty =
            DependencyProperty.Register("MaximizedOrNormalVisibility", typeof(Visibility), _ownerType, new PropertyMetadata(Visibility.Visible));
        #endregion

        #region IsShowInTaskBar 是否在任务通知栏显示
        public bool IsShowInTaskBar
        {
            get { return (bool)GetValue(IsShowInTaskBarProperty); }
            set { SetValue(IsShowInTaskBarProperty, value); }
        }

        public static readonly DependencyProperty IsShowInTaskBarProperty =
            DependencyProperty.Register("IsShowInTaskBar", typeof(bool), _ownerType, new PropertyMetadata(false)); 
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

        protected override void OnInitialized(EventArgs e)
        {
            if (IsShowInTaskBar)
            {
                NotifyIcon = new NotifyIcon
                {
                    Visible = true,                   
                };
            }
            base.OnInitialized(e);
        }

        protected override void OnClosed(EventArgs e)
        {           
            base.OnClosed(e);
            if (IsShowInTaskBar)
                NotifyIcon.Dispose();
        }

        
    }  
}
