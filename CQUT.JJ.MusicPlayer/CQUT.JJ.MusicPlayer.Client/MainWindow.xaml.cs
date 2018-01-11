using CQUT.JJ.MusicPlayer.Controls.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CQUT.JJ.MusicPlayer.Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : JmWindow
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void JmWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //GlassHelper.ExtendGlassFrame(this, new Thickness(-1));
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            GlassHelper.ExtendGlassFrame(this, new Thickness(-1));
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MARGINS
        {
            public MARGINS(Thickness t)
            {
                Left = (int)t.Left;
                Right = (int)t.Right;
                Top = (int)t.Top;
                Bottom = (int)t.Bottom;
            }
            public int Left;
            public int Right;
            public int Top;
            public int Bottom;
        }

        public class GlassHelper
        {
            [DllImport("dwmapi.dll", PreserveSig = false)]
            static extern void DwmExtendFrameIntoClientArea(
                IntPtr hWnd, ref MARGINS pMarInset);
            [DllImport("dwmapi.dll", PreserveSig = false)]
            static extern bool DwmIsCompositionEnabled();

            public static bool ExtendGlassFrame(Window window, Thickness margin)
            {
                if (!DwmIsCompositionEnabled())
                    return false;

                IntPtr hwnd = new WindowInteropHelper(window).Handle;
                if (hwnd == IntPtr.Zero)
                    throw new InvalidOperationException(
                    "The Window must be shown before extending glass.");

                // Set the background to transparent from both the WPF and Win32 perspectives  
                window.Background = Brushes.Transparent;
                HwndSource.FromHwnd(hwnd).CompositionTarget.BackgroundColor = Colors.Transparent;

                MARGINS margins = new MARGINS(margin);
                DwmExtendFrameIntoClientArea(hwnd, ref margins);
                return true;
            }
        }
    }
}
