using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Xna.Framework;

namespace WP_Geocaching.View.Compass
{
    using Microsoft.Devices.Sensors;
    using System.Windows.Threading;
    using WP_Geocaching.Model;

    public partial class CompassView : UserControl, ICompassView
    {
        private CompassManager compassManager; 
        private double needleDirection;


        public CompassView()
        {
            InitializeComponent();  
            compassManager = new CompassManager(this);
        }        

        public void SetDirection(double direction)
        {
            needleDirection = direction;
            DrawCompass();
        }

        private void DrawCompass()
        {
            // Update the line objects to graphically display the headings
            double centerX = LayoutRoot.ActualWidth / 2.0;
            double centerY = LayoutRoot.ActualHeight / 2.0;

            compassLine.X2 = centerX - centerY * Math.Sin(MathHelper.ToRadians((float)needleDirection));
            compassLine.Y2 = centerY - centerY * Math.Cos(MathHelper.ToRadians((float)needleDirection));
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            LogManager.Log("LayoutRoot_Loaded");
            compassManager.Start();        
        }

        //TODO: don't called on win-button click
        private void LayoutRoot_Unloaded(object sender, RoutedEventArgs e)
        {
            LogManager.Log("LayoutRoot_Unloaded");
           
            // Stop data acquisition from the compass.
            compassManager.Stop();         
        }

    }
}
