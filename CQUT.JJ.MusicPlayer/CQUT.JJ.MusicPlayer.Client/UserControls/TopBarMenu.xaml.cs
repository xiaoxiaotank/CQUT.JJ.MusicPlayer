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

namespace CQUT.JJ.MusicPlayer.Client.UserControls
{
    /// <summary>
    /// TopBarMenu.xaml 的交互逻辑
    /// </summary>
    public partial class TopBarMenu : UserControl
    {
        private static readonly string _qmSearchPageName = "OnlineMusic/QMSearchPage.xaml";

        private static readonly string _jmSearchPageName = "OnlineMusic/JMSearchPage.xaml";

        private static string _lastQMSearchKey = string.Empty;

        private static string _lastJMSearchKey = string.Empty;

        public TopBarMenu()
        {
            InitializeComponent();

            //数据请求事件
            MusicSearchInfoChangedUtil.QMRequestEvent += MusicSearchInfoChangedUtil_QMRequestEvent;
            //后退按钮
            MusicPageSwitchedUtil.MusicPageEnablePreviousSwitchedEvent += MusicPageEnablePreviousSwitchedEvent;
            //前进按钮
            MusicPageSwitchedUtil.MusicPageEnableNextSwitchedEvent += MusicPageEnableNextSwitchedEvent;
        }

        private void MusicPageEnablePreviousSwitchedEvent(object sender, MusicPageSwitchedEventArgs e) => BtnPreviousPage.IsEnabled = e.CanSwitch;

        private void MusicPageEnableNextSwitchedEvent(object sender, MusicPageSwitchedEventArgs e) => BtnNextPage.IsEnabled = e.CanSwitch;



        private void CmbSearch_SearchBtnClick(object sender, RoutedEventArgs e)
        {           
            if(FindResource("MusicSource") is Grid musicSourceGrid)
            {
                RadioButton qmSource = null;
                if((qmSource = musicSourceGrid.GetChildObjectByName<RadioButton>("RdQM")) != null)
                {
                    if(qmSource.IsChecked == true)
                    {
                        MusicPageChangedUtil.Invoke(_qmSearchPageName, true);                      
                    }
                    else
                    {

                    }
                }
            }
        }

        private void MusicSearchInfoChangedUtil_QMRequestEvent(object sender, MusicSearchInfoRequestArgs e)
        {
            GetQMusics(e.TargetPageNumber);
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
                    var musicInfoOfPageModel = GetQMusicInfoOfPageModel(page);
                    if (musicInfoOfPageModel == null) return;
                    MusicSearchInfoChangedUtil.InvokeFromQMSearchChanged(musicInfoOfPageModel,page);
                }
                catch (Exception ex)
                {
                    MusicSearchInfoChangedUtil.InvokeFromQMSearchChanged(null,page, false, ex.Message);
                }
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private MusicInfoOfPageModel GetQMusicInfoOfPageModel(int currentPageNumber)
        {
            var searchKey = CmbSearch.Text;
            if (!IsValidateSearchKey(ref searchKey)) return null;

            var musicInfoOfPageModel = new MusicInfoOfPageModel() { CurrentPageNumber = currentPageNumber };            
            var url = $"https://y.qq.com/portal/search.html#page={currentPageNumber}&searchid=1&remoteplace=txt.yqq.top&t=song&w={searchKey}";

            try
            {
                var doc = GetHtmlDocument(url, TimeSpan.FromSeconds(10), html =>
                 {
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

                if (lastPagerNode?.Attributes["class"].Value.Equals("current") == true)
                    musicInfoOfPageModel.TotalPageNumber = Convert.ToInt32(lastPagerNode.InnerHtml);
                else
                    musicInfoOfPageModel.TotalPageNumber = Convert.ToInt32(doc.DocumentNode.SelectNodes("//a[@class='js_pageindex']").LastOrDefault()?.Attributes["data-index"].Value);

                var songCount = MathUtil.GetMin(songList.Count, singerList.Count, albumList.Count, timeDurationList.Count);
                for (int i = 0; i < songCount; i++)
                {
                    var id = songList[i].Attributes["href"].Value.Substring(songList[i].Attributes["href"].Value.LastIndexOf('/') + 1).Replace(".html", "");
                    if (id.Length != 14) continue;
                    musicInfoOfPageModel.MusicInfoList.Add(new QMInfoModel()
                    {
                        Id = id,
                        Name = songList[i].Attributes["title"].Value,
                        Singer = string.Join("/", singerList[i].SelectNodes("./a[@class='singer_name']").Select(n => n.Attributes["title"]).Select(n => n.Value)),
                        AlbumInfo = new QMAlbumInfoModel()
                        {
                            Id = albumList[i].Attributes["data-albumid"].Value,
                            Name = albumList[i].Attributes["title"].Value
                        },
                        TimeDuration = timeDurationList[i].FirstChild.InnerText
                    });
                }

                return musicInfoOfPageModel;
            }
            catch
            {
                throw new Exception("服务器崩溃了 --!");
            }
        }

        private HtmlDocument GetHtmlDocument(string url, TimeSpan outTime, Func<string, bool> isScriptCompleted = null)
        {
            try
            {
                var htmlWeb = new HtmlWeb()
                {
                    BrowserTimeout = outTime
                };
                return htmlWeb.LoadFromBrowser(url, isScriptCompleted);
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
                if (string.IsNullOrWhiteSpace(_lastQMSearchKey)) return false;

                searchKey = _lastQMSearchKey;
                CmbSearch.Focus();
                CmbSearch.Text = _lastQMSearchKey;
            }
            else
                _lastQMSearchKey = searchKey;
            return true;
        }

        #endregion

      
    }
}
