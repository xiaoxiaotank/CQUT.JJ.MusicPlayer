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

namespace CQUT.JJ.MusicPlayer.Controls.Controls
{
    /// <summary>
    /// ListViewStyle.xaml 的交互逻辑
    /// </summary>
    public partial class JmListView : ListView
    {
        private static readonly Type _ownerType = typeof(JmListView);

        static JmListView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(_ownerType, new FrameworkPropertyMetadata(_ownerType));
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new JmListViewItem();
        }
    }
}
