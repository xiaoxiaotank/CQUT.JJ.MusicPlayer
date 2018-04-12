using CQUT.JJ.MusicPlayer.Client.Pages.UserMusicList;
using CQUT.JJ.MusicPlayer.Client.Utils;
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

        public static readonly IUserMusicListService UserMusicListService;

        static UserMusicNavigtion()
        {
            UserMusicListService = new UserMusicListService();
        }

        public UserMusicNavigtion()
        {
           

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
                    return UserMusicListService.GetUserMusicListByUserId(App.User.Id)
                        .OrderByDescending(u => u.CreationTime);
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
                        tabItem.EditBoxLostFocus += TabItem_EditBoxLostFocus;
                        tabItem.MouseUp += UserMusicListSelectionChanged;
                        tabItem.ContextMenu = GetContextMenu();
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
                SetSelectedTabItem(originalSource);
                ChangePage();
            }
        }


        private void UserMusicListSelectionChanged(object sender, MouseButtonEventArgs e)
        {
            if(sender is JmTabItem originalSource && !originalSource.Equals(_selectedTabItem))
            {
                SetSelectedTabItem(originalSource);

                ChangeUserMusicListPage(Convert.ToInt32(originalSource.Tag));
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
            Sviewer.JmScrollToElement(SpICreate);
        }

        private void TabItem_EditBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if(sender is JmTabItem tabItem)
            {
                tabItem.Editable = false;
                var name = tabItem.Header.ToString();
                if (tabItem.Tag == null)
                {
                    var userMusicListInfo = new UserMusicListContract()
                    {
                        UserId = App.User.Id,
                        Name = name
                    };
                    userMusicListInfo = UserMusicListService.Create(userMusicListInfo);
                    tabItem.Tag = userMusicListInfo.Id;
                    tabItem.MouseUp += UserMusicListSelectionChanged;
                    tabItem.ContextMenu = GetContextMenu();
                    SetSelectedTabItem(tabItem);
                    ChangeUserMusicListPage(Convert.ToInt32(userMusicListInfo.Id));
                }                   
                else
                {
                    var id = Convert.ToInt32(tabItem.Tag);
                    UserMusicListService.Update(id, name);
                }                
            }
        }

        /// <summary>
        /// 创建新的歌单名
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 更改页面
        /// </summary>
        private void ChangePage()
        {
            if(_selectedTabItem != null)
                MusicPageChangedUtil.Invoke($@"{_selectedTabItem.PageOfColumnName}/{ _selectedTabItem.PageName}");
        }

        /// <summary>
        /// 设置选中的TabItem
        /// </summary>
        /// <param name="tabItem"></param>
        private void SetSelectedTabItem(JmTabItem tabItem)
        {
            tabItem.IsSelected = true;
            if (_selectedTabItem != null)
                _selectedTabItem.IsSelected = false;
            _selectedTabItem = tabItem;
        }


        private ContextMenu GetContextMenu()
        {
            return FindResource("ContextMenu") as ContextMenu;
        }

        private void CtxPlay_Click(object sender, RoutedEventArgs e)
        {
            ContextMenuStartPlayMusicUtil.Invoke();
        }

        private void CtxDelete_Click(object sender, RoutedEventArgs e)
        {
            if (((sender as JmMenuItem)?.Parent as JmContextMenu)?.PlacementTarget is JmTabItem parent)
            {
                var id = Convert.ToInt32(parent.Tag);
                UserMusicListService.Delete(id);
                parent.Visibility = Visibility.Collapsed;
            }
            
        }

        private void CtxRename_Click(object sender, RoutedEventArgs e)
        {
            if (((sender as JmMenuItem)?.Parent as JmContextMenu)?.PlacementTarget is JmTabItem parent)
            {
                var id = Convert.ToInt32(parent.Tag);
                parent.Editable = true;
            }
        }

        private void ChangeUserMusicListPage(int userMusicListId)
        {
            var frame = App.Current.MainWindow.FindVisualChild<Frame>();
            var userMusicListPage = new UserMusicList(userMusicListId);
            frame.Navigate(userMusicListPage);
        }
    }
}
