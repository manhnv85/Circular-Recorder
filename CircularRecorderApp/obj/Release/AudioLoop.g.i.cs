﻿#pragma checksum "Z:\E\Personal\Apps\CircularRecorderApp\CircularRecorderApp\AudioLoop.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C86F90146B6FC2CFDAC84B0D4BFAF9C2"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Expression.Shapes;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace CircularRecorderApp {
    
    
    public partial class AudioLoop : System.Windows.Controls.UserControl {
        
        internal System.Windows.Media.Animation.Storyboard _animation;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Shapes.Ellipse Backgroundellipse;
        
        internal Microsoft.Expression.Shapes.Arc arc_1;
        
        internal Microsoft.Expression.Shapes.Arc arc_2;
        
        internal System.Windows.Shapes.Ellipse FrontEllipse;
        
        internal System.Windows.Controls.TextBlock TimeTextBlock;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/CircularRecorderApp;component/AudioLoop.xaml", System.UriKind.Relative));
            this._animation = ((System.Windows.Media.Animation.Storyboard)(this.FindName("_animation")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.Backgroundellipse = ((System.Windows.Shapes.Ellipse)(this.FindName("Backgroundellipse")));
            this.arc_1 = ((Microsoft.Expression.Shapes.Arc)(this.FindName("arc_1")));
            this.arc_2 = ((Microsoft.Expression.Shapes.Arc)(this.FindName("arc_2")));
            this.FrontEllipse = ((System.Windows.Shapes.Ellipse)(this.FindName("FrontEllipse")));
            this.TimeTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("TimeTextBlock")));
        }
    }
}

