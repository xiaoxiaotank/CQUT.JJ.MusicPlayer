﻿using System;
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
    public partial class JmTabItem : TabItem
    {
        private readonly static Type _ownerType = typeof(JmTabItem);

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
            DependencyProperty.Register("IconMargin", typeof(Thickness), _ownerType, new PropertyMetadata(new Thickness())); 
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

        #region 是否可编辑

        public bool Editable
        {
            get { return (bool)GetValue(EditableProperty); }
            set
            {
                SetValue(EditableProperty, value);
                RaiseEvent(new RoutedEventArgs(EditableChangedEvent));
            }
        }

        public static readonly DependencyProperty EditableProperty =
            DependencyProperty.Register(nameof(Editable), typeof(bool), _ownerType, new PropertyMetadata(false));
        #endregion

        #region 编辑框丢失焦点事件
        public event RoutedEventHandler EditBoxLostFocus
        {
            add { AddHandler(EditBoxLostFocusEvent, value); }
            remove { RemoveHandler(EditBoxLostFocusEvent, value); }
        }

        public static readonly RoutedEvent EditBoxLostFocusEvent =
            EventManager.RegisterRoutedEvent(nameof(EditBoxLostFocus), RoutingStrategy.Bubble, typeof(RoutedEventArgs), _ownerType);
        #endregion

        #region 可编辑状态改变事件
        public event RoutedEventHandler EditableChanged
        {
            add { AddHandler(EditableChangedEvent, value); }
            remove { RemoveHandler(EditableChangedEvent, value); }
        }

        public static readonly RoutedEvent EditableChangedEvent =
            EventManager.RegisterRoutedEvent(nameof(EditableChanged), RoutingStrategy.Bubble, typeof(RoutedEventArgs), _ownerType); 
        #endregion

        static JmTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(_ownerType, new FrameworkPropertyMetadata(_ownerType));
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            if(!IsSelected)
                IsSelected = true;
            base.OnPreviewMouseUp(e);
        }

        public void InvokeEditBoxLostFocusEvent()
        {

        }
    }
}
