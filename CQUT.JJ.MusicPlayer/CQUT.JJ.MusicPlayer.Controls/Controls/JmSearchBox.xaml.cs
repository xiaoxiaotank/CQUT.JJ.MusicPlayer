using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CQUT.JJ.MusicPlayer.Controls.Controls
{
    /// <summary>
    /// JmSearchComboBox.xaml 的交互逻辑
    /// </summary>
    public partial class JmSearchBox : ComboBox
    {
        private static readonly Type _ownerType = typeof(JmSearchBox);

        #region BorderCornerRadius 边框圆角
        public CornerRadius BorderCornerRadius
        {
            get { return (CornerRadius)GetValue(BorderCornerRadiusProperty); }
            set { SetValue(BorderCornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty BorderCornerRadiusProperty =
            DependencyProperty.Register("BorderCornerRadius", typeof(CornerRadius), _ownerType, new PropertyMetadata(new CornerRadius(0)));
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

        #region TextForeground 文本颜色
        public Brush TextForeground
        {
            get { return (Brush)GetValue(TextForegroundProperty); }
            set { SetValue(TextForegroundProperty, value); }
        }

        public static readonly DependencyProperty TextForegroundProperty =
            DependencyProperty.Register("TextForeground", typeof(Brush), _ownerType, new PropertyMetadata(new SolidColorBrush(Colors.Black)));
        #endregion

        #region SearchButtonWidth 搜索按钮宽度
        public double SearchButtonWidth
        {
            get { return (double)GetValue(SearchButtonWidthProperty); }
            set { SetValue(SearchButtonWidthProperty, value); }
        }

        public static readonly DependencyProperty SearchButtonWidthProperty =
            DependencyProperty.Register("SearchButtonWidth", typeof(double), _ownerType, new PropertyMetadata(30d));
        #endregion

        #region SearchButtonIconCode 搜索按钮图标码
        public string SearchButtonIconCode
        {
            get { return (string)GetValue(SearchButtonIconCodeProperty); }
            set { SetValue(SearchButtonIconCodeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchButtonIconCode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchButtonIconCodeProperty =
            DependencyProperty.Register("SearchButtonIconCode", typeof(string), _ownerType, new PropertyMetadata("\ue611"));
        #endregion

        #region PlaceholderIcon 提示符图标
        public string PlaceholderIcon
        {
            get { return (string)GetValue(PlaceholderIconProperty); }
            set { SetValue(PlaceholderIconProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderIconProperty =
            DependencyProperty.Register("PlaceholderIcon", typeof(string), _ownerType, new PropertyMetadata(string.Empty)); 
        #endregion

        #region Placeholder 提示符
        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), _ownerType, new PropertyMetadata(string.Empty));
        #endregion

        #region PlaceholderIconFontFamily 提示符字体
        public FontFamily PlaceholderIconFontFamily
        {
            get { return (FontFamily)GetValue(PlaceholderIconFontFamilyProperty); }
            set { SetValue(PlaceholderIconFontFamilyProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderIconFontFamilyProperty =
            DependencyProperty.Register("PlaceholderIconFontFamily", typeof(FontFamily), _ownerType, new PropertyMetadata(new FontFamily("微软雅黑")));
        #endregion

        #region PlaceholderHorizontalAlignment 提示符水平基准
        public HorizontalAlignment PlaceholderHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(PlaceholderHorizontalAlignmentProperty); }
            set { SetValue(PlaceholderHorizontalAlignmentProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderHorizontalAlignmentProperty =
            DependencyProperty.Register("PlaceholderHorizontalAlignment", typeof(HorizontalAlignment), _ownerType, new PropertyMetadata(HorizontalAlignment.Left));
        #endregion

        #region PlaceholderOpacity 提示符透明度
        public double PlaceholderOpacity
        {
            get { return (double)GetValue(PlaceholderOpacityProperty); }
            set { SetValue(PlaceholderOpacityProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderOpacityProperty =
            DependencyProperty.Register("PlaceholderOpacity", typeof(double), _ownerType, new PropertyMetadata(0.9));
        #endregion

        #region ItemTitle 条目标题
        public string ItemTitle
        {
            get { return (string)GetValue(ItemTitleProperty); }
            set { SetValue(ItemTitleProperty, value); }
        }

        public static readonly DependencyProperty ItemTitleProperty =
            DependencyProperty.Register("ItemTitle", typeof(string), _ownerType, new PropertyMetadata(string.Empty));
        #endregion

        #region ItemBackground 条目背景色
        public Brush ItemBackground
        {
            get { return (Brush)GetValue(ItemBackgroundProperty); }
            set { SetValue(ItemBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ItemBackgroundProperty =
            DependencyProperty.Register("ItemBackground", typeof(Brush), _ownerType, new PropertyMetadata(new SolidColorBrush(Colors.White)));
        #endregion

        #region ItemTitleForeground 条目标题前景色
        public Brush ItemTitleForeground
        {
            get { return (Brush)GetValue(ItemTitleForegroundProperty); }
            set { SetValue(ItemTitleForegroundProperty, value); }
        }

        public static readonly DependencyProperty ItemTitleForegroundProperty =
            DependencyProperty.Register("ItemTitleForeground", typeof(Brush), _ownerType, new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        #endregion

        #region ItemTitleOpacity 条目标题透明度
        public double ItemTitleOpacity
        {
            get { return (double)GetValue(ItemTitleOpacityProperty); }
            set { SetValue(ItemTitleOpacityProperty, value); }
        }

        public static readonly DependencyProperty ItemTitleOpacityProperty =
            DependencyProperty.Register("ItemTitleOpacity", typeof(double), _ownerType, new PropertyMetadata(1d));
        #endregion

        #region IsOpenHeaderContainer 是否开启头部容器
        public bool IsOpenHeaderContainer
        {
            get { return (bool)GetValue(IsOpenHeaderContainerProperty); }
            set { SetValue(IsOpenHeaderContainerProperty, value); }
        }

        public static readonly DependencyProperty IsOpenHeaderContainerProperty =
            DependencyProperty.Register("IsOpenHeaderContainer", typeof(bool), _ownerType, new PropertyMetadata(false));
        #endregion

        #region HeaderContent 头部内容
        public FrameworkElement HeaderContent
        {
            get { return (FrameworkElement)GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }

        public static readonly DependencyProperty HeaderContentProperty =
            DependencyProperty.Register("HeaderContent", typeof(FrameworkElement), _ownerType, new PropertyMetadata(null)); 
        #endregion




        static JmSearchBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(_ownerType, new FrameworkPropertyMetadata(_ownerType));
        }

        public JmSearchBox()
        {
            //不要隐藏IsEditable属性，会导致Popup的值无法选中
            IsEditable = true;
        }
    }
}
