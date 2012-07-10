﻿using System;
using WP_Geocaching.View.Compass;
using Microsoft.Devices.Sensors;
using WP_Geocaching.Model.Utils;
using System.Windows.Threading;
using System.Collections.Generic;

namespace WP_Geocaching.Model
{
    public class SmoothCompassManager
    {
        private const int LongSleep = 60;
        private const int DefaultSleep = 20;

        private const float ArrivedEps = 0.65f;
        private const float LeavedEps = 2.5f;
        private const float SpeedEps = 0.55f;

        private readonly Compass compass;
        private readonly DispatcherTimer timer;

        private double goalDirection;
        private double needleDirection;
        private double speed;

        private List<ICompassView> subscribers = new List<ICompassView>();

        private bool isArrived; // The needle has not arrived the goalDirection

        private static SmoothCompassManager instance;

        public static SmoothCompassManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SmoothCompassManager();
                }
                return instance;
            }
        }

        public void AddSubscriber(ICompassView compassView)
        {
            subscribers.Add(compassView);

            this.Start();
        }

        public void RemoveSubscriber(ICompassView compassView)
        {
            subscribers.Remove(compassView);

            if (subscribers.Count == 0)
                this.Stop();
        }

        private SmoothCompassManager()
        {
            //this.compassView = compassView;
            if (!Compass.IsSupported)
            {
                //TODO: show something
            }
            else
            {
                // Initialize the timer and add Tick event handler, but don't start it yet.
                timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(DefaultSleep) };
                timer.Tick += TimerTick;

                // Instantiate the compass.
                compass = new Compass { TimeBetweenUpdates = TimeSpan.FromMilliseconds(DefaultSleep) };
                compass.CurrentValueChanged += CompassCurrentValueChanged;
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (IsNeedPainting())
            {
                isArrived = false;
                double difference = CompassHelper.CalculateNormalDifference(needleDirection, goalDirection);
                speed = CalculateSpeed(difference, speed);
                needleDirection = needleDirection + speed;

                foreach (ICompassView c in subscribers)
                    c.SetDirection(needleDirection);
            }
            else
            {
                isArrived = true;
            }

            timer.Interval = TimeSpan.FromMilliseconds(isArrived ? LongSleep : DefaultSleep);
        }

        private void Start()
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

        private void CompassCurrentValueChanged(object sender, SensorReadingEventArgs<CompassReading> e)
        {
            double newDirection = e.SensorReading.TrueHeading;
            double difference = newDirection - goalDirection;
            difference = CompassHelper.NormalizeAngle(difference);

            newDirection = goalDirection + difference / 4; // TODO extract constant
            newDirection = CompassHelper.NormalizeAngle(newDirection);

            goalDirection = newDirection;
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

        private void Stop()
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