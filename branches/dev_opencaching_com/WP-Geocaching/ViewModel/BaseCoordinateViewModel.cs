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

namespace WP_Geocaching.ViewModel
{
    public class BaseCoordinateViewModel
    {
        protected const string DotPosition = "0.";
        protected const int MinLatitude = -90;
        protected const int MaxLatitude = 90;
        protected const int MinLongitude = -180;
        protected const int MaxLongitude = 180;
        protected const double Eps = 0.000001;

        protected int degrees;
        protected bool positive;
        protected CoordinateType coordinateType;

        public string Degrees
        {
            get
            {
                return degrees.ToString();
            }
        }

        public bool SetDegrees(string value)
        {
            int deg;

            if (int.TryParse(value, out deg))
            {
                if (coordinateType == CoordinateType.Lat)
                {
                    if (deg > MinLatitude && deg < MaxLatitude)
                    {
                        degrees = deg;
                        positive = degrees > 0;
                        return true;
                    }
                }
                else
                {
                    if (deg > MinLongitude && deg < MaxLongitude)
                    {
                        degrees = deg;
                        positive = degrees > 0;
                        return true;
                    }
                }
            }

            return false;
        }
    }
}