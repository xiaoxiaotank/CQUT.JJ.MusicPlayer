using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CQUT.JJ.MusicPlayer.Controls.ControlAttachProperties
{
    public static class InputControlAttachProperty
    {
        private static readonly Type _ownerType = typeof(InputControlAttachProperty);

        #region Placeholder 占位符
        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty
            .RegisterAttached("Placeholder", typeof(string), _ownerType, new FrameworkPropertyMetadata(string.Empty));

        public static string GetPlaceholder(DependencyObject obj)
        {
            return (string)obj.GetValue(PlaceholderProperty);
        }

        public static void SetPlaceholder(DependencyObject obj, string value)
        {
            obj.SetValue(PlaceholderProperty, value);
        }
        #endregion
    }
}
