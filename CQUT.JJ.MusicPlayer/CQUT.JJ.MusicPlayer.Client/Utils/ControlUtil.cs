using CQUT.JJ.MusicPlayer.Controls.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CQUT.JJ.MusicPlayer.Client.Utils
{
    public static class ControlUtil
    {
        public static void JmScrollToControl(this JmScrollViewer scrollViewer,Control control)
        {
            // 获取要定位之前 ScrollViewer 目前的滚动位置
            var currentScrollPosition = scrollViewer.VerticalOffset;
            var point = new Point(0, currentScrollPosition);

            // 计算出目标位置并滚动
            var targetPosition = control.TransformToVisual(scrollViewer).Transform(point);
            scrollViewer.ScrollToVerticalOffset(targetPosition.Y);
        }
    }
}
