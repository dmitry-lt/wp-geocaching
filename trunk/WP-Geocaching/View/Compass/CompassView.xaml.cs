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

    public partial class CompassView : UserControl
    {
        Compass compass;
        DispatcherTimer timer;

        double compassHeading;
        double headingAccuracy;
        bool isDataValid;

        public CompassView()
        {
            InitializeComponent();

            if (!Compass.IsSupported)
            {
                //TODO: show something
            }
            else
            {
                // Initialize the timer and add Tick event handler, but don't start it yet.
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(30);
                timer.Tick += new EventHandler(timer_Tick);
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {

            if (isDataValid)
            {
                //TODO: show smt message
            }


            // Update the line objects to graphically display the headings
            double centerX = LayoutRoot.ActualWidth / 2.0;
            double centerY = LayoutRoot.ActualHeight / 2.0;

            compassLine.X2 = centerX - centerY * Math.Sin(MathHelper.ToRadians((float)compassHeading));
            compassLine.Y2 = centerY - centerY * Math.Cos(MathHelper.ToRadians((float)compassHeading));
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("LayoutRoot_Loaded");

            if (compass == null)
            {
                // Instantiate the compass.
                compass = new Compass();


                // Specify the desired time between updates. The sensor accepts
                // intervals in multiples of 20 ms.
                compass.TimeBetweenUpdates = TimeSpan.FromMilliseconds(20);

                compass.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<CompassReading>>(compass_CurrentValueChanged);
            }

            try
            {
                compass.Start();
                timer.Start();
            }
            catch (InvalidOperationException)
            {
                //TODO: show message
            }
        }

        void compass_CurrentValueChanged(object sender, SensorReadingEventArgs<CompassReading> e)
        {
            // Note that this event handler is called from a background thread
            // and therefore does not have access to the UI thread. To update 
            // the UI from this handler, use Dispatcher.BeginInvoke() as shown.
            // Dispatcher.BeginInvoke(() => { statusTextBlock.Text = "in CurrentValueChanged"; });

            isDataValid = compass.IsDataValid;

            compassHeading = e.SensorReading.TrueHeading;
            headingAccuracy = Math.Abs(e.SensorReading.HeadingAccuracy);
        }

        //TODO: don't called on win-button click
        private void LayoutRoot_Unloaded(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine ("LayoutRoot_Unloaded");

            // Stop data acquisition from the compass.
            compass.Stop();
            timer.Stop();
        }

    }
}
