﻿using CQUT.JJ.MusicPlayer.Controls.Controls;
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

namespace CQUT.JJ.MusicPlayer.Client.UserControls
{
    /// <summary>
    /// UserMusicNavigtion.xaml 的交互逻辑
    /// </summary>
    public partial class UserMusicNavigtion : UserControl
    {
        private static JmTabItem _selectedTabItem = null;

        public UserMusicNavigtion()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TiMusicHall.IsSelected = true;
            _selectedTabItem = TiMusicHall;
        }


        private void TabItem_SelectionChanged(object sender, MouseButtonEventArgs e)
        {
            if (sender is JmTabItem originalSource && !originalSource.Equals(_selectedTabItem))
            {
                _selectedTabItem.IsSelected = false;
                _selectedTabItem = originalSource;               
            }
        }
    }
}
