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

namespace CQUT.JJ.MusicPlayer.Client.UserControls
{
    /// <summary>
    /// TopBarMenu.xaml 的交互逻辑
    /// </summary>
    public partial class TopBarMenu : UserControl
    {
        private static readonly string _qmSearchPageName = "OnlineMusic/QMSearchPage.xaml";

        private static readonly string _jmSearchPageName = "OnlineMusic/JMSearchPage.xaml";

        public TopBarMenu()
        {
            InitializeComponent();
        }

        private void CmbSearch_SearchBtnClick(object sender, RoutedEventArgs e)
        {           
            if(FindResource("MusicSource") is Grid musicSourceGrid)
            {
                RadioButton qmSource = null;
                if((qmSource = musicSourceGrid.GetChildObjectByName<RadioButton>("RdQM")) != null)
                {
                    if(qmSource.IsChecked == true)
                    {
                        MusicPageChangedUtil.Invoke(_qmSearchPageName,true);
                        var qmSongInfoModels = new List<QMSongInfoModel>();
                        var searchKey = CmbSearch.Text;
                        var url = $"https://y.qq.com/portal/search.html#page=1&searchid=1&remoteplace=txt.yqq.top&t=song&w={searchKey}";
                        var htmlWeb = new HtmlWeb()
                        {
                            BrowserTimeout = TimeSpan.FromSeconds(30)
                        };
                        try
                        {
                            var doc = htmlWeb.LoadFromBrowser(url);
                            var songList = doc.DocumentNode.SelectNodes("//a[@class='js_song']");
                            var singerList = doc.DocumentNode.SelectNodes("//div[@class='songlist__artist']");
                            var albumList = doc.DocumentNode.SelectNodes("//a[@class='album_name']");
                            var timeDurationList = doc.DocumentNode.SelectNodes("//div[@class='songlist__time']");
                            var count = MathUtil.GetMin(songList.Count, singerList.Count, albumList.Count, timeDurationList.Count);
                            for (int i = 0; i < count; i++)
                            {
                                var id = songList[i].Attributes["href"].Value.Substring(songList[i].Attributes["href"].Value.LastIndexOf('/') + 1).Replace(".html", "");
                                if (id.Length != 14) continue;
                                qmSongInfoModels.Add(new QMSongInfoModel()
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
                            MusicSearchInfoChangedUtil.InvokeFromQM(qmSongInfoModels);
                        }
                        catch(Exception ex)
                        {
                            return;
                        }                       
                    }
                    else
                    {

                    }
                }
            }
        }
    }
}
