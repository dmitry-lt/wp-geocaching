using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using WP_Geocaching.View.Compass;
using Microsoft.Devices.Sensors;
using WP_Geocaching.Model.utils;
using System.Windows.Threading;

namespace WP_Geocaching.Model
{
    public class CompassManager
    {
        private const int LONG_SLEEP = 120;
        private const int DEFAULT_SLEEP = 40;

        private const float ARRIVED_EPS = 0.65f;
        private const float LEAVED_EPS = 2.5f;
        private const float SPEED_EPS = 0.55f;

        private ICompassView compassView;
        private Compass compass;
        private DispatcherTimer timer;

        private double goalDirection = 0;
        private double headingAccuracy;
        private bool isDataValid;


        public CompassManager(ICompassView compassView)
        {
            this.compassView = compassView;

            if (!Compass.IsSupported)
            {
                //TODO: show something
            }
            else
            {
                // Initialize the timer and add Tick event handler, but don't start it yet.
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(DEFAULT_SLEEP);
                timer.Tick += new EventHandler(timer_Tick);
            }
        }

        private double speed = 0;
        private double needleDirection = 0;
        private bool isArrived = false; // The needle has not arrived the goalDirection


        private void timer_Tick(object sender, EventArgs e)
        {
            double currentDirection = goalDirection;
            bool needPainting = IsNeedPainting(isArrived, speed, needleDirection, currentDirection);

            if (needPainting)
            {
                isArrived = false;
                double difference = CompassHelper.CalculateNormalDifference(needleDirection, currentDirection);
                speed = calculateSpeed(difference, speed);
                currentDirection = needleDirection + speed;
                needleDirection = currentDirection;

                compassView.SetDirection(goalDirection);
            }
            else
            {
                isArrived = true;
            }

            timer.Interval = TimeSpan.FromMilliseconds(isArrived ? LONG_SLEEP : DEFAULT_SLEEP);
        }

        public void Start()
        {
            // Instantiate the compass.
            compass = new Compass();

            // Specify the desired time between updates. The sensor accepts
            // intervals in multiples of DEFAULT_SLEEP ms.
            compass.TimeBetweenUpdates = TimeSpan.FromMilliseconds(DEFAULT_SLEEP);
            compass.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<CompassReading>>(compass_CurrentValueChanged);

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

        private double averageDirection = 0; //TODO: is it need

        private void compass_CurrentValueChanged(object sender, SensorReadingEventArgs<CompassReading> e)
        {
            // Note that this event handler is called from a background thread
            // and therefore does not have access to the UI thread. To update 
            // the UI from this handler, use Dispatcher.BeginInvoke() as shown.
            // Dispatcher.BeginInvoke(() => { statusTextBlock.Text = "in CurrentValueChanged"; });
            isDataValid = compass.IsDataValid;

            double newDirection = e.SensorReading.TrueHeading;
            double difference = newDirection - averageDirection;
            difference = CompassHelper.normalizeAngle(difference);

            newDirection = averageDirection + difference / 4; // TODO extract constant
            newDirection = CompassHelper.normalizeAngle(newDirection);

            goalDirection = averageDirection = newDirection;
            headingAccuracy = Math.Abs(e.SensorReading.HeadingAccuracy);
        }


        private double calculateSpeed(double difference, double oldSpeed)
        {
            oldSpeed = oldSpeed * 0.75f;
            oldSpeed += difference / 25.0f;

            return oldSpeed;
        }

        private bool IsNeedPainting(bool isArrived, double speed, double needleDirection, double goalDirection)
        {
            if (isArrived)
            {
                return Math.Abs(needleDirection - goalDirection) > LEAVED_EPS;
            }
            else
            {
                return Math.Abs(needleDirection - goalDirection) > ARRIVED_EPS || Math.Abs(speed) > SPEED_EPS;
            }
        }


        public void Stop()
        {
            compass.Stop();
            timer.Stop();
        }

    }
}
