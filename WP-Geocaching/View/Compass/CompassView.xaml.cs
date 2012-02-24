using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xna.Framework;

namespace WP_Geocaching.View.Compass
{
    using Model; //TODO: MVVM!?

    public partial class CompassView : UserControl, ICompassView
    {
        private const int NeedleLength = 200;
        private readonly SmoothCompassManager smoothCompassManager;
        private double needleDirection;

        public CompassView()
        {
            InitializeComponent();
            smoothCompassManager = new SmoothCompassManager(this);
        }

        public void SetDirection(double direction)
        {
            needleDirection = direction;
            DrawCompass();
        }

        private DateTime time;
        private void DrawCompass()
        {
            // Update the line objects to graphically display the headings
            double centerX = CompassRose.ActualWidth / 2.0;
            double centerY = CompassRose.ActualHeight / 2.0;

            CacheNiddle.X2 = centerX - NeedleLength * Math.Sin(MathHelper.ToRadians((float)needleDirection));
            CacheNiddle.Y2 = centerY - NeedleLength * Math.Cos(MathHelper.ToRadians((float)needleDirection));

            CompassRoseRotating.Angle = -needleDirection;
            FpsTextBox.Text = "" + 1000 / (DateTime.Now - time).Milliseconds;
            time = DateTime.Now;
        }

        private void LayoutRootLoaded(object sender, RoutedEventArgs e)
        {
            LogManager.Log("LayoutRoot_Loaded");
            smoothCompassManager.Start();
        }

        //TODO: don't called on win-button click
        private void LayoutRootUnloaded(object sender, RoutedEventArgs e)
        {
            LogManager.Log("LayoutRoot_Unloaded");

            // Stop data acquisition from the compass.
            smoothCompassManager.Stop();
        }

    }
}
