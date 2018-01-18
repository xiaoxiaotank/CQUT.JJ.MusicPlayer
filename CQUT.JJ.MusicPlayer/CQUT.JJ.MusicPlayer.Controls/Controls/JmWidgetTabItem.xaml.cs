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
    /// JmTabItem.xaml 的交互逻辑
    /// </summary>
    public partial class JmWidgetTabItem : TabItem
    {
        private readonly static Type _ownerType = typeof(JmWidgetTabItem);

        #region Icon 图标
        public object Icon
        {
            get { return (object)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(object), _ownerType, new PropertyMetadata(string.Empty));

        #endregion

        #region IconMargin 图标外边距
        public Thickness IconMargin
        {
            get { return (Thickness)GetValue(IconMarginProperty); }
            set { SetValue(IconMarginProperty, value); }
        }

        public static readonly DependencyProperty IconMarginProperty =
            DependencyProperty.Register("IconMargin", typeof(Thickness), _ownerType, new PropertyMetadata(new Thickness(0))); 
        #endregion

        #region CornerRadius 弧角半径
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), _ownerType, new PropertyMetadata(new CornerRadius()));
        #endregion

        #region WidgetVisibility 是否展示饰品
        public bool IsShowWidget
        {
            get { return (bool)GetValue(IsShowWidgetProperty); }
            set { SetValue(IsShowWidgetProperty, value); }
        }

        public static readonly DependencyProperty IsShowWidgetProperty =
            DependencyProperty.Register("IsShowWidget", typeof(bool), _ownerType, new PropertyMetadata(true));
        #endregion

        #region WidgetHeight 视频高度
        public double WidgetHeight
        {
            get { return (double)GetValue(WidgetHeightProperty); }
            set { SetValue(WidgetHeightProperty, value); }
        }

        public static readonly DependencyProperty WidgetHeightProperty =
            DependencyProperty.Register("WidgetHeight", typeof(double), _ownerType, new PropertyMetadata(2d)); 
        #endregion

        #region PageName 页面名字，含后缀
        public string PageName
        {
            get { return (string)GetValue(PageNameProperty); }
            set { SetValue(PageNameProperty, value); }
        }

        public static readonly DependencyProperty PageNameProperty =
            DependencyProperty.Register("PageName", typeof(string), _ownerType, new PropertyMetadata(string.Empty));
        #endregion

        #region PageOfColumnName 页面所属栏目名
        public string PageOfColumnName
        {
            get { return (string)GetValue(PageOfColumnNameProperty); }
            set { SetValue(PageOfColumnNameProperty, value); }
        }

        public static readonly DependencyProperty PageOfColumnNameProperty =
            DependencyProperty.Register("PageOfColumnName", typeof(string), _ownerType, new PropertyMetadata(string.Empty)); 
        #endregion
       

        static JmWidgetTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(_ownerType, new FrameworkPropertyMetadata(_ownerType));
        }

        public JmWidgetTabItem()
        {
            if (!IsShowWidget)
                WidgetHeight = 0;
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            if(!IsSelected)
                IsSelected = true;
            base.OnPreviewMouseUp(e);
        }
    }
}
