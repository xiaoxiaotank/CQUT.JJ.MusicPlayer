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

        private new bool IsEditable
        {
            get { return (bool)GetValue(IsEditableProperty); }
        }

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


        public JmSearchBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(_ownerType, new FrameworkPropertyMetadata(_ownerType));
        }
    }
}
