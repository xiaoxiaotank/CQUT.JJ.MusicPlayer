using CQUT.JJ.MusicPlayer.Controls.Controls;
using CQUT.JJ.MusicPlayer.Controls.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CQUT.JJ.MusicPlayer.Controls.ControlHelpers
{
    public partial class JmWindowHelper : ResourceDictionary
    {
        #region Events
        private void JmWindow_Close(object sender, RoutedEventArgs e)
        {
            ((sender as FrameworkElement).TemplatedParent as JmWindow).Close();
        }

        private void JmWindow_Maximized(object sender, RoutedEventArgs e)
        {
            var jmWindow = (sender as FrameworkElement).TemplatedParent as JmWindow;
            if (jmWindow.WindowState != WindowState.Maximized)
                jmWindow.WindowState = WindowState.Maximized;
        }

        private void JmWindow_Minimized(object sender, RoutedEventArgs e)
        {
            var jmWindow = (sender as FrameworkElement).TemplatedParent as JmWindow;
            if(jmWindow.WindowState != WindowState.Minimized)
                jmWindow.WindowState = WindowState.Minimized;
        }

        private void JmWindow_Normal(object sender, RoutedEventArgs e)
        {
            var jmWindow = (sender as FrameworkElement).TemplatedParent as JmWindow;
            if (jmWindow.WindowState != WindowState.Normal)
                jmWindow.WindowState = WindowState.Normal;
        }
        

        private void JmWindow_Move(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                ((sender as FrameworkElement).TemplatedParent as JmWindow).DragMove();
        }

        private void Window_AddToolBarMenuItems(object sender,RoutedEventArgs e)
        {
            var panel = sender as Panel;
            var menuItems = (panel.TemplatedParent as JmWindow).ToolBarMenuItems;
            foreach (var menuItem in menuItems)
            {
                var icon = menuItem.Content as TextBlock;
                if (icon != null)
                    icon.Style = panel.FindResource("DefalultToolBarMenuItemStyle") as Style;
                panel.Children.Add(menuItem);
            }
        }

        #endregion
    }
}
