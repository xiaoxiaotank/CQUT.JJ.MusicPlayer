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
using CQUT.JJ.MusicPlayer.Models;
using HtmlAgilityPack;
using CQUT.JJ.MusicPlayer.Client.Utils;
using CQUT.JJ.MusicPlayer.Client.Utils.EventUtils;
using System.Threading;
using System.IO;
using CQUT.JJ.MusicPlayer.Models.JM.Common;
using CQUT.JJ.MusicPlayer.WCFService;
using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
using CQUT.JJ.MusicPlayer.Models.DataContracts.Search;
using CQUT.JJ.MusicPlayer.Models.DataContracts;
using System.Web;
using CQUT.JJ.MusicPlayer.Client.Pages.OnlineMusic.Search;

namespace CQUT.JJ.MusicPlayer.Client.UserControls
{
    /// <summary>
    /// TopBarMenu.xaml 的交互逻辑
    /// </summary>
    public partial class TopBarMenu : UserControl
    {
        private static readonly string _qmSearchPageName = "OnlineMusic/Search/QMSearchPage.xaml";

        private static readonly string _jmSearchPageName = "OnlineMusic/Search/MusicSearchPage.xaml";

        private static readonly string _searchRecordsPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SearchRecords.bat");

        private static readonly int _maxSearchRecordsCount = 8;

        private static string _lastSearchKey = string.Empty;

        public TopBarMenu()
        {
            InitializeComponent();

            //QM数据请求事件
            MusicSearchInfoChangedUtil.QMRequestEvent += MusicSearchInfoChangedUtil_QMRequestEvent;
            //JM数据请求事件
            MusicSearchInfoChangedUtil.JMRequestEvent += MusicSearchInfoChangedUtil_JMRequestEvent;
            //后退按钮
            MusicPageSwitchedUtil.MusicPageEnablePreviousSwitchedEvent += MusicPageEnablePreviousSwitchedEvent;
            //前进按钮
            MusicPageSwitchedUtil.MusicPageEnableNextSwitchedEvent += MusicPageEnableNextSwitchedEvent;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCmbSearchItems();
        }

        private void MusicPageEnablePreviousSwitchedEvent(object sender, MusicPageSwitchedEventArgs e) => BtnPreviousPage.IsEnabled = e.CanSwitch;

        private void MusicPageEnableNextSwitchedEvent(object sender, MusicPageSwitchedEventArgs e) => BtnNextPage.IsEnabled = e.CanSwitch;

        /// <summary>
        /// 点击搜索按钮->切换页面->页面中请求数据->获取数据->传送到页面->展示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbSearch_SearchBtnClick(object sender, RoutedEventArgs e)
        {           
            if(FindResource("MusicSource") is Grid musicSourceGrid)
            {
                RadioButton qmSource = null;
                if((qmSource = musicSourceGrid.GetChildObjectByName<RadioButton>("RdQM")) != null)
                {
                    var searchKey = CmbSearch.Text;
                    if (!IsValidateSearchKey(ref searchKey)) return;
                    SaveSearchRecords(searchKey);

                    if (qmSource.IsChecked == true)
                    {
                        MusicPageChangedUtil.Invoke(_qmSearchPageName, true);
                    }
                    else 
                    {
                        var musicSearchPage = new MusicSearchPage(_lastSearchKey);
                        (App.Current.MainWindow.FindName("FMusicPage") as Frame).Navigate(musicSearchPage);                        
                    }
                    NonNavPageDisplayedUtil.Invoke();
                }
            }
        }

        private void MusicSearchInfoChangedUtil_QMRequestEvent(object sender, BaseMusicSearchInfoRequestArgs e)
        {
            GetQMusics(e.TargetPageNumber);
        }


        private void MusicSearchInfoChangedUtil_JMRequestEvent(object sender, MusicSearchInfoRequestArgs e)
        {
            GetJMusics(e.Type, e.TargetPageNumber,e.Size);
        }


        private void BtnRefreshPage_Click(object sender, RoutedEventArgs e)
        {
            MusicPageRefreshUtil.Invoke();
        }

        private void BtnPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            MusicPageSwitchedUtil.InvokeOfPrevious();
        }

        private void BtnNextPage_Click(object sender, RoutedEventArgs e)
        {
            MusicPageSwitchedUtil.InvokeOfNext();
        }

        #region Helpers
        private void GetQMusics(int page)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    var musicInfoOfPageModel = GetQMusicInfoOfPageModel(page, _lastSearchKey);
                    if (musicInfoOfPageModel == null) return;
                    MusicSearchInfoChangedUtil.InvokeFromQMSearchChanged(musicInfoOfPageModel,page);
                }
                catch (Exception ex)
                {
                    MusicSearchInfoChangedUtil.InvokeFromQMSearchChanged(null,page, false, ex.Message);
                }
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void GetJMusics(MusicRequestType type,int page,int size)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    var musicInfoOfPageModel = GetJMusicInfoOfPageModel(type, _lastSearchKey, page, size);
                    if (musicInfoOfPageModel == null) return;
                    MusicSearchInfoChangedUtil.InvokeFromJMSearchChanged(musicInfoOfPageModel, page);
                }
                catch (Exception ex)
                {
                    MusicSearchInfoChangedUtil.InvokeFromJMSearchChanged(null, page, false, ex.Message);
                }
            });
        }

        private void SaveSearchRecords(string searchKey)
        {
            var searchRocords = new List<string>() { searchKey };
            foreach (var item in CmbSearch.Items)
            {
                if (item is string data && !searchRocords.Contains(data))
                    searchRocords.Add(data);
            }
            FileUtil.WriteToFileByLine(_searchRecordsPath, searchRocords.Take(_maxSearchRecordsCount).ToArray());

            LoadCmbSearchItems();
        }

        private QMusicsOfPageModel GetQMusicInfoOfPageModel(int currentPageNumber,string searchKey)
        {           
            var musicInfoOfPageModel = new QMusicsOfPageModel() { CurrentPageNumber = currentPageNumber };      
            searchKey = HttpUtility.UrlEncode(searchKey);
            var url = $"https://y.qq.com/portal/search.html#page={currentPageNumber}&searchid=1&remoteplace=txt.yqq.top&t=song&w={searchKey}";

            try
            {              
                var doc = GetHtmlDocument(url, TimeSpan.FromSeconds(10), html =>
                 {
                     if (currentPageNumber == 1)
                         return html.Contains("js_song")
                             && html.Contains("songlist__artist")
                             && html.Contains("album_name")
                             && html.Contains("songlist__time");
                     else
                         return html.Contains("js_song")
                             && html.Contains("songlist__artist")
                             && html.Contains("album_name")
                             && html.Contains("songlist__time")
                             && html.Contains("js_pageindex");
                 });
                var songList = doc.DocumentNode.SelectNodes("//a[@class='js_song']");
                var singerList = doc.DocumentNode.SelectNodes("//div[@class='songlist__artist']");
                var albumList = doc.DocumentNode.SelectNodes("//a[@class='album_name']");
                var timeDurationList = doc.DocumentNode.SelectNodes("//div[@class='songlist__time']");
                var lastPagerNode = doc.DocumentNode.SelectNodes("//div[@class='mod_page_nav js_pager']")?.FirstOrDefault()?.ChildNodes?.LastOrDefault();

                if(lastPagerNode == null)
                    musicInfoOfPageModel.TotalPageNumber = 1;
                else
                {
                    if (lastPagerNode?.Attributes["class"].Value.Equals("current") == true)
                        musicInfoOfPageModel.TotalPageNumber = Convert.ToInt32(lastPagerNode.InnerHtml);
                    else
                        musicInfoOfPageModel.TotalPageNumber = Convert.ToInt32(doc.DocumentNode.SelectNodes("//a[@class='js_pageindex']").LastOrDefault()?.Attributes["data-index"].Value);
                }

                var songCount = MathUtil.GetMin(songList.Count, singerList.Count, albumList.Count, timeDurationList.Count);
                for (int i = 0; i < songCount; i++)
                {
                    var id = songList[i].Attributes["href"].Value.Substring(songList[i].Attributes["href"].Value.LastIndexOf('/') + 1).Replace(".html", "");
                    if (id.Length != 14) continue;

                    musicInfoOfPageModel.MusicInfoList.Add(new QMInfoModel()
                    {
                        Id = id,
                        SourcePath = songList[i].Attributes["href"].Value,
                        Name = songList[i].Attributes["title"].Value,
                        Singer = string.Join("/", singerList[i].SelectNodes("./a[@class='singer_name']").Select(n => n.Attributes["title"]).Select(n => n.Value)),
                        AlbumInfo = new QMAlbumInfoModel()
                        {
                            Id = albumList[i].Attributes["data-albumid"].Value,
                            Name = albumList[i].Attributes["title"].Value
                        },
                        TimeDuration = timeDurationList[i].FirstChild.InnerText,
                    });
                }

                return musicInfoOfPageModel;
            }
            catch(Exception ex)
            {
                throw new Exception("服务器崩溃了 --!");
            }
        }

        /// <summary>
        /// 获取某一页的歌曲类型搜索结果
        /// </summary>
        /// <param name="type"></param>
        /// <param name="searchKey"></param>
        /// <param name="currentPageNumber"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private PageResult GetJMusicInfoOfPageModel(MusicRequestType type, string searchKey, int currentPageNumber, int size)
        {
            ISearchService musicSearchService = new SearchService();
            var result = musicSearchService.Search(type,searchKey, currentPageNumber, size);
            return result;
        }

        private HtmlDocument GetHtmlDocument(string url, TimeSpan outTime, Func<string, bool> isScriptCompleted = null)
        {
            try
            {
                var htmlWeb = new HtmlWeb()
                {
                    BrowserTimeout = outTime,
                    UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.140 Safari/537.36",
                    CaptureRedirect = true
                };
                return isScriptCompleted == null ? 
                    htmlWeb.LoadFromBrowser(url) : htmlWeb.LoadFromBrowser(url, isScriptCompleted);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool IsValidateSearchKey(ref string searchKey)
        {
            if (string.IsNullOrWhiteSpace(searchKey))
            {
                if (string.IsNullOrWhiteSpace(_lastSearchKey)) return false;

                searchKey = _lastSearchKey;
                CmbSearch.Focus();
                CmbSearch.Text = _lastSearchKey;
            }
            else
                _lastSearchKey = searchKey;
            return true;
        }

        private void LoadCmbSearchItems()
        {
            var text = CmbSearch.Text;
            CmbSearch.Items.Clear();
            CmbSearch.Text = text;
            foreach (var data in FileUtil.ReadFromFileByLine(_searchRecordsPath).Take(_maxSearchRecordsCount))
            {
                CmbSearch.Items.Add(data);
            }            
        }
        #endregion


    }
}
