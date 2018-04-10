﻿using CQUT.JJ.MusicPlayer.Client.Utils;
using CQUT.JJ.MusicPlayer.Client.Utils.EventUtils;
using CQUT.JJ.MusicPlayer.Controls.Controls;
using CQUT.JJ.MusicPlayer.Models.DataContracts.Common;
using CQUT.JJ.MusicPlayer.WCFService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        private readonly IUserMusicListService _userMusicListService;

        public UserMusicNavigtion()
        {
            _userMusicListService = new UserMusicListService();

            InitializeComponent();
            NonNavPageDisplayedUtil.NonNavPageDisplayedEvent += NonNavPageDisplayed;

            UserStateChangedUtil.UserStateChangedEvent += UserStateChanged;
        }


        private void NonNavPageDisplayed(object sender, EventArgs e)
        {
            if(_selectedTabItem != null)
            {
                _selectedTabItem.IsSelected = false;
                _selectedTabItem = null;
            }           
        }

        private async void UserStateChanged(object sender, EventArgs e)
        {
            if(App.User == null)
            {
                TabILike.Visibility = Visibility.Collapsed;
                SpICreate.Visibility = Visibility.Collapsed;
                SpUserMusicList.Visibility = Visibility.Collapsed;
            }
            else
            {
                TabILike.Visibility = Visibility.Visible;
                SpICreate.Visibility = Visibility.Visible;
                TbLoading.Visibility = Visibility.Visible;
                var userMusicList = await Task.Factory.StartNew(() =>
                {
                    return _userMusicListService.GetUserMusicListByUserId(App.User.Id);
                });

                Dispatcher.Invoke(() => 
                {
                    foreach (var list in userMusicList)
                    {
                        var tabItem = new JmTabItem()
                        {
                            Header = list.Name,
                            Icon = new TextBlock()
                            {
                                Text = "\ue62e",
                                FontSize = 15
                            },
                            Tag = list.Id
                        };
                        tabItem.MouseUp += UserMusicListSelectionChanged;
                        SpUserMusicList.Children.Add(tabItem);
                    }
                });
                TbLoading.Visibility = Visibility.Collapsed;
                SpUserMusicList.Visibility = Visibility.Visible;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TiMusicHall.IsSelected = true;
            _selectedTabItem = TiMusicHall;
            ChangePage();
        }


        private void TabItem_SelectionChanged(object sender, MouseButtonEventArgs e)
        {
            if (sender is JmTabItem originalSource && !originalSource.Equals(_selectedTabItem))
            {
                if(_selectedTabItem != null)
                    _selectedTabItem.IsSelected = false;
                _selectedTabItem = originalSource;
                ChangePage();
            }
        }


        private void UserMusicListSelectionChanged(object sender, MouseButtonEventArgs e)
        {
            if(sender is JmTabItem originalSource && !originalSource.Equals(_selectedTabItem))
            {
                if (_selectedTabItem != null)
                    _selectedTabItem.IsSelected = false;
                _selectedTabItem = originalSource;
                MusicPageChangedUtil.Invoke($"UserMusicList/UserMusicList.xaml");
            }
            
        }



        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            var temp = SpUserMusicList.Children;
            var tabItem = new JmTabItem()
            {
                Header = CreateNewMusicListName(temp),
                Icon = new TextBlock()
                {
                    Text = "\ue62e",
                    FontSize = 15
                },
                Editable = true
            };            
            tabItem.EditBoxLostFocus += TabItem_EditBoxLostFocus;
            var children = new List<UIElement>() { tabItem };
            foreach (UIElement item in temp)
            {
                children.Add(item);
            }
            SpUserMusicList.Children.Clear();
            foreach (var item in children)
            {
                SpUserMusicList.Children.Add(item);
            }
            Sviewer.JmScrollToControl(tabItem);
        }

        private void TabItem_EditBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if(sender is JmTabItem tabItem)
            {
                tabItem.Editable = false;
                var userMusicListInfo = new UserMusicListInfo()
                {
                    UserId = App.User.Id,
                    Name = tabItem.Header.ToString()
                };
                userMusicListInfo = _userMusicListService.Create(userMusicListInfo);
                tabItem.Tag = userMusicListInfo.Id;
                tabItem.MouseUp += UserMusicListSelectionChanged;
            }
        }

        private string CreateNewMusicListName(UIElementCollection temp)
        {
            var i = 1;            
            var header = string.Empty;
            while (true)
            {
                header = "新建歌单" + i++;
                var isDifferent = true;
                foreach (var item in temp)
                {
                    if (item is JmTabItem tab && tab.Header.Equals(header))
                    {
                        isDifferent = false;
                        break;
                    }
                }
                if(isDifferent)
                    return header;
            }           
        }

        private void ChangePage()
        {
            if(_selectedTabItem != null)
                MusicPageChangedUtil.Invoke($@"{_selectedTabItem.PageOfColumnName}/{ _selectedTabItem.PageName}");
        }

    }
}
