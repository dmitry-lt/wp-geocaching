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

namespace WP_Geocaching.ViewModel
{
    public class DegCoordinateViewModel : BaseCoordinateViewModel
    {
        private const string FormatDegreesFraction = "{0:0.000000}";

        private double degreesFraction;

        public string DegreesFraction
        {
            get
            {
                return String.Format(FormatDegreesFraction, degreesFraction).Substring(2);
            }
        }

        public bool SetDegreesFraction(string value)
        {
            string val = DotPosition + value;
            double degFraction;

            if (double.TryParse(val, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out degFraction) && val != DotPosition)
            {
                degreesFraction = degFraction;
                return true;
            }

            return false;
        }

        public DegCoordinateViewModel(double coordinate, CoordinateType type)
        {
            coordinateType = type;
            positive = coordinate > 0;
            degrees = (int)coordinate;
            degreesFraction = Math.Abs(coordinate - (int)coordinate);
        }

        public double ToCoordinate()
        {
            if (positive)
            {
                return degrees + degreesFraction;
            }

            return degrees - degreesFraction;
        }
    }
}