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
    public class MinCoordinateViewModel : BaseCoordinateViewModel
    {
        private const string FormatMinutesFraction = "{0:0.000}";

        private int minutes;
        private double minutesFraction;

        public string Minutes
        {
            get
            {
                return minutes.ToString();
            }
        }

        public string MinutesFraction
        {
            get
            {
                return String.Format(FormatMinutesFraction, minutesFraction).Substring(2);
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

        public bool SetMinutesFraction(string value)
        {
            string val = DotPosition + value;
            double minFraction;

            if (double.TryParse(val, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out minFraction) && val != DotPosition)
            {
                minutesFraction = minFraction;
                return true;
            }

            return false;
        }

        public MinCoordinateViewModel(double coordinate, CoordinateType type)
        {
            coordinateType = type;
            positive = coordinate > 0 ? true : false;
            degrees = (int)coordinate;
            minutes = Math.Abs((int)((coordinate - degrees) * 60));
            minutesFraction = Math.Abs((coordinate - degrees)) * 60 - minutes;

            if (1 - minutesFraction < Eps)
            {
                minutes++;
                minutesFraction = 0;
            }
        }

        public double ToCoordinate()
        {
            if (positive)
            {
                return degrees + (minutes + minutesFraction) / 60.0;
            }

            return degrees - (minutes + minutesFraction) / 60.0;
        }
    }
}