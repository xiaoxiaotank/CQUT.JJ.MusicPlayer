using CQUT.JJ.MusicPlayer.Controls.Commands;
using CQUT.JJ.MusicPlayer.Controls.Controls;
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
    public partial class JmTabItemHelper
    {
        public LostFocusCommand LostFocusCommand { get; private set; }

        public JmTabItemHelper()
        {
            LostFocusCommand = new LostFocusCommand(element => LostEditBoxFocus(element));
        }

        private void LostEditBoxFocus(object element)
        {
            var txtBox = element as TextBox;
            (txtBox.TemplatedParent as FrameworkElement)?.Focus();
        }

        public void EditBox_Loaded(object sender, RoutedEventArgs e)
        {
            var txtBox = sender as TextBox;
            //设置此处以使得Command有效
            txtBox.DataContext = this;
        }

        public void EditBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var templateParent = (sender as TextBox).TemplatedParent as JmTabItem;
            templateParent.RaiseEvent(new RoutedEventArgs(JmTabItem.EditBoxLostFocusEvent));
        }

        private void EditBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var txtBox = sender as TextBox;
            if (txtBox.IsVisible)
            {
                txtBox.Focus();
                txtBox.SelectAll();
            }
        }
    }

    
}
