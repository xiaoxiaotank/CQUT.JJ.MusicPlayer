using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace CQUT.JJ.MusicPlayer.Controls.Helpers
{
    internal static class AeroEffctHelper
    {
        [DllImport("dwmapi.dll")]
        static extern void DwmEnableBlurBehindWindow(IntPtr hwnd, ref DWM_BLURBEHIND blurBehind);

        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        private readonly static int _osVersionNumber = Environment.OSVersion.Version.Major;

        public static void OpenAero(this Window window)
        {
            if(window != null)
            {
                switch(_osVersionNumber)
                {
                    case 6:
                        window.OpenAeroFromWin7();
                        break;
                    case 10:
                        window.OpenAeroFromWin10();
                        break;
                }
            }
        }

        private static void OpenAeroFromWin7(this Window window)
        {
            var windowPtr = new WindowInteropHelper(window).Handle;

            var blur = new DWM_BLURBEHIND()
            {
                dwFlags = 0x00000001 | 0x00000002,
                fEnable = true
            };

            DwmEnableBlurBehindWindow(windowPtr, ref blur);
        }

        private static void OpenAeroFromWin10(this Window window)
        {
            var windowPtr = new WindowInteropHelper(window).Handle;

            var accent = new AccentPolicy()
            {
                AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND,
                AccentFlags = 0x20 | 0x40 | 0x80 | 0x100,
                //GradientColor = 0x000000FF,
                //AnimationId = 
            };
            var accentStructSize = Marshal.SizeOf(accent);

            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData()
            {
                Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
                SizeOfData = accentStructSize,
                Data = accentPtr
            };
            SetWindowCompositionAttribute(windowPtr, ref data);

            Marshal.FreeHGlobal(accentPtr);
        }
    }

    struct DWM_BLURBEHIND
    {
        public uint dwFlags;
        public bool fEnable;
        public IntPtr hRgnBlur;
        public bool fTransitionOnMaximized;
    }

    internal enum AccentState
    {
        ACCENT_DISABLED = 0,
        ACCENT_ENABLE_GRADIENT = 1,
        ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
        ACCENT_ENABLE_BLURBEHIND = 3,
        ACCENT_INVALID_STATE = 4
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct AccentPolicy
    {
        public AccentState AccentState;
        public int AccentFlags;
        public int GradientColor;
        public int AnimationId;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WindowCompositionAttributeData
    {
        public WindowCompositionAttribute Attribute;
        public IntPtr Data;
        public int SizeOfData;
    }

    internal enum WindowCompositionAttribute
    {
        // ...
        WCA_ACCENT_POLICY = 19
        // ...
    }
}
