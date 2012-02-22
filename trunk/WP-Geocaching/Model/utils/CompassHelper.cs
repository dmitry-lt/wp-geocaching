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

namespace WP_Geocaching.Model.utils
{
    public class CompassHelper
    {

        public static double CalculateNormalDifference(double lastDirection, double currentDirection)
        {
            double difference = currentDirection - lastDirection;
            return normalizeAngle(difference);
        }

        public static double normalizeAngle(double angle)
        {
            angle %= 360;
            if (angle < -180)
            {
                angle += 360;
            }
            if (angle > 180)
            {
                angle -= 360;
            }
            return angle;
        }
    }
}
