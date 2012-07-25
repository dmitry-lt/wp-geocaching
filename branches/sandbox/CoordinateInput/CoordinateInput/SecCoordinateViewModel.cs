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
using System.Globalization;

namespace CoordinateInput
{
    public class SecCoordinateViewModel : BaseCoordinateViewModel
    {
        private const string FormatSecondsFraction = "{0:0.000}";

        private int minutes;
        private int seconds;
        private double secondsFraction;

        public string Minutes
        {
            get
            {
                return minutes.ToString();
            }
        }

        public string Seconds
        {
            get
            {
                return seconds.ToString();
            }
        }

        public string SecondsFraction
        {
            get
            {
                return String.Format(FormatSecondsFraction, secondsFraction).Substring(2);
            }
        }

        public bool SetMinutes(string value)
        {
            int min;

            if (int.TryParse(value, out min))
            {
                if (min >= 0 && min < 60)
                {
                    minutes = min;
                    return true;
                }
            }

            return false;
        }

        public bool SetSeconds(string value)
        {
            int sec;

            if (int.TryParse(value, out sec))
            {
                if (sec >= 0 && sec < 60)
                {
                    seconds = sec;
                    return true;
                }
            }

            return false;
        }

        public bool SetSecondsFraction(string value)
        {
            string val = DotPosition + value;
            double secFraction;

            if (double.TryParse(val, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out secFraction) && val != DotPosition)
            {
                secondsFraction = secFraction;
                return true;
            }

            return false;
        }

        public SecCoordinateViewModel(double coordinate, CoordinateType type)
        {
            coordinateType = type;
            positive = coordinate > 0 ? true : false;
            degrees = (int)coordinate;
            double minutesFraction = Math.Abs(coordinate - degrees) * 60;
            minutes = (int)minutesFraction;
            seconds = (int)((minutesFraction - minutes) * 60);
            secondsFraction = ((minutesFraction - minutes) * 60) - seconds;

            if (1 - (minutesFraction - minutes) < Eps)
            {
                minutes++;
                seconds = 0;
                secondsFraction = 0;
            }

            if (1 - ((minutesFraction - minutes) * 60 - seconds) < Eps)
            {
                seconds++;
                secondsFraction = 0;
            }
        }

        public double ToCoordinate()
        {
            if (positive)
            {
                return degrees + (minutes / 60.0 + (seconds + secondsFraction) / 3600.0);
            }

            return degrees - (minutes / 60.0 + (seconds + secondsFraction) / 3600.0);
        }
    }
}