﻿using System;
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
    /// JmCheckBox.xaml 的交互逻辑
    /// </summary>
    public partial class JmCheckBox : CheckBox
    {
        private static readonly Type _ownerType = typeof(JmCheckBox);




        public Brush CheckedMarkBackground
        {
            get { return (Brush)GetValue(CheckedMarkBackgroundProperty); }
            set { SetValue(CheckedMarkBackgroundProperty, value); }
        }

        public static readonly DependencyProperty CheckedMarkBackgroundProperty =
            DependencyProperty.Register(nameof(CheckedMarkBackground), typeof(Brush), _ownerType, new PropertyMetadata(new SolidColorBrush(Colors.SkyBlue)));



        static JmCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(_ownerType, new FrameworkPropertyMetadata(_ownerType));
        }

        public JmCheckBox()
        {
            
        }
    }
}
