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
    public partial class JmListViewItem : ListViewItem
    {
        private static readonly Type _ownerType = typeof(JmListViewItem);

        #region IsActivated 是否处于激活状态
        public bool IsActivated
        {
            get { return (bool)GetValue(IsActivatedProperty); }
            set { SetValue(IsActivatedProperty, value); }
        }

        public static readonly DependencyProperty IsActivatedProperty =
            DependencyProperty.Register("IsActivated", typeof(bool), _ownerType, new PropertyMetadata(false));

        #endregion


        static JmListViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(_ownerType, new FrameworkPropertyMetadata(_ownerType));
        }

    }
}
