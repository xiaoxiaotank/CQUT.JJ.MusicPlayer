﻿using CQUT.JJ.MusicPlayer.Controls.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace CQUT.JJ.MusicPlayer.Controls.ControlHelpers
{
    public partial class JmSearchBoxHelper : ResourceDictionary
    {
        private static JmSearchBox _templateParent = null;

        private void TextBox_Loaded(object sender,RoutedEventArgs e)
        {
            var txtBox = sender as TextBox;
            _templateParent = txtBox.TemplatedParent as JmSearchBox;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            (_templateParent.Template.FindName("BtnSearch", _templateParent) as FrameworkElement).Visibility = Visibility.Visible;
            (_templateParent.Template.FindName("SpPlaceholder", _templateParent) as FrameworkElement).Visibility = Visibility.Hidden;
            (_templateParent.Template.FindName("PART_Popup", _templateParent) as Popup).IsOpen = true;
            
        }

        private void TextBox_LostFocus(object sender,RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace((sender as TextBox).Text))
            {
                (_templateParent.Template.FindName("BtnSearch", _templateParent) as FrameworkElement).Visibility = Visibility.Collapsed;
                (_templateParent.Template.FindName("SpPlaceholder", _templateParent) as FrameworkElement).Visibility = Visibility.Visible;
            }
        }


    }
}
