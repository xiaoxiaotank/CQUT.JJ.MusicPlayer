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
    public partial class JmTabItemHelper
    {
        public void EditBox_Loaded(object sender, RoutedEventArgs e)
        {
            var txtBox = sender as TextBox;
            txtBox.Focus();
            txtBox.SelectAll();
        }

        public void EditBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var templateParent = (sender as TextBox).TemplatedParent as JmTabItem;
            templateParent.RaiseEvent(new RoutedEventArgs(JmTabItem.EditBoxLostFocusEvent));
        }
    }
}
