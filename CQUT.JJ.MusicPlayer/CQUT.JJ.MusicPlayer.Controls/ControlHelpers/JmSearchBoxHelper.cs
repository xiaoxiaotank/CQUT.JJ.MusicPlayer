using CQUT.JJ.MusicPlayer.Controls.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CQUT.JJ.MusicPlayer.Controls.ControlHelpers
{
    public partial class JmSearchBoxHelper : ResourceDictionary
    {
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var parent = (sender as TextBox).TemplatedParent as JmSearchBox;
            (parent.Template.FindName("BtnSearch", parent) as JmTransparentButton).Visibility = Visibility.Visible;
        }

        private void TextBox_LostFocus(object sender,RoutedEventArgs e)
        {
            var parent = (sender as TextBox).TemplatedParent as JmSearchBox;
            (parent.Template.FindName("BtnSearch", parent) as JmTransparentButton).Visibility = Visibility.Collapsed;
        }
    }
}
