using System;
using WP_Geocaching.View.Compass;
using Microsoft.Devices.Sensors;
using WP_Geocaching.Model.utils;
using System.Windows.Threading;

namespace WP_Geocaching.Model
{
    public class SmoothCompassManager
    {
        private const int LongSleep = 120;
        private const int DefaultSleep = 40;

        private const float ArrivedEps = 0.65f;
        private const float LeavedEps = 2.5f;
        private const float SpeedEps = 0.55f;

        private ICompassView compassView;
        private Compass compass;
        private DispatcherTimer timer;

        private double goalDirection;
        private double needleDirection;
        private double speed;

        private bool isArrived = false; // The needle has not arrived the goalDirection


        public SmoothCompassManager(ICompassView compassView)
        {
            this.compassView = compassView;

            if (!Compass.IsSupported)
            {
                //TODO: show something
            }
            else
            {
                // Initialize the timer and add Tick event handler, but don't start it yet.
                timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(DefaultSleep) };
                timer.Tick += new EventHandler(TimerTick);

                // Instantiate the compass.
                compass = new Compass { TimeBetweenUpdates = TimeSpan.FromMilliseconds(DefaultSleep) };
                compass.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<CompassReading>>(CompassCurrentValueChanged);
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            double currentDirection = goalDirection;

            if (IsNeedPainting())
            {
                isArrived = false;
                double difference = CompassHelper.CalculateNormalDifference(needleDirection, goalDirection);
                speed = CalculateSpeed(difference, speed);
                needleDirection = needleDirection + speed;
                compassView.SetDirection(needleDirection);
            }
            else
            {
                isArrived = true;
            }

            timer.Interval = TimeSpan.FromMilliseconds(isArrived ? LongSleep : DefaultSleep);
        }

        public void Start()
        {
            if (!Compass.IsSupported)
            {
                return;
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

        private double averageDirection; //TODO: is it need

        private void CompassCurrentValueChanged(object sender, SensorReadingEventArgs<CompassReading> e)
        {
            double newDirection = e.SensorReading.TrueHeading;
            double difference = newDirection - averageDirection;
            difference = CompassHelper.normalizeAngle(difference);

            newDirection = averageDirection + difference / 4; // TODO extract constant
            newDirection = CompassHelper.normalizeAngle(newDirection);

            goalDirection = averageDirection = newDirection;
        }


        private double CalculateSpeed(double difference, double oldSpeed)
        {
            oldSpeed = oldSpeed * 0.75f;
            oldSpeed += difference / 25.0f;

            return oldSpeed;
        }

        private bool IsNeedPainting()
        {
            if (isArrived)
            {
                return Math.Abs(needleDirection - goalDirection) > LeavedEps;
            }
            else
            {
                return Math.Abs(needleDirection - goalDirection) > ArrivedEps || Math.Abs(speed) > SpeedEps;
            }
        }

        public void Stop()
        {
            if (!Compass.IsSupported)
            {
                return;
            }
            compass.Stop();
            timer.Stop();
        }

    }
}
