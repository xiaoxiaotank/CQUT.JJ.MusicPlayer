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
    /// JmRoundButton.xaml 的交互逻辑
    /// </summary>
    public partial class JmRoundButton : Button
    {
        private static readonly Type _ownerType = typeof(JmRoundButton);

        #region Radius 半径
        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), _ownerType, new PropertyMetadata(50d)); 
        #endregion

        static JmRoundButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(_ownerType, new FrameworkPropertyMetadata(_ownerType));
        }
    }
}
