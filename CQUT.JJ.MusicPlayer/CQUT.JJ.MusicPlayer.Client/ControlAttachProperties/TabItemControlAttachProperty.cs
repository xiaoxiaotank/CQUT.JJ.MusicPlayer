using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CQUT.JJ.MusicPlayer.Client.ControlAttachProperties
{
    public static class TabItemControlAttachProperty
    {
        private static readonly Type _ownerType = typeof(TabItemControlAttachProperty);



        #region GetPageNameWithoutExtension 无后缀页面名
        public static string GetPageNameWithoutExtension(DependencyObject obj)
        {
            return (string)obj.GetValue(PageNameWithoutExtensionProperty);
        }

        public static void SetPageNameWithoutExtension(DependencyObject obj, string value)
        {
            obj.SetValue(PageNameWithoutExtensionProperty, value);
        }

        public static readonly DependencyProperty PageNameWithoutExtensionProperty =
            DependencyProperty.RegisterAttached("PageNameWithoutExtension", typeof(string), _ownerType, new PropertyMetadata(string.Empty));
        #endregion



        #region GetDataType 数据类型
        public static string GetDataType(DependencyObject obj)
        {
            return (string)obj.GetValue(DataTypeProperty);
        }

        public static void SetDataType(DependencyObject obj, string value)
        {
            obj.SetValue(DataTypeProperty, value);
        }

        public static readonly DependencyProperty DataTypeProperty =
            DependencyProperty.RegisterAttached("DataType", typeof(string), _ownerType, new PropertyMetadata(string.Empty)); 
        #endregion


    }
}
