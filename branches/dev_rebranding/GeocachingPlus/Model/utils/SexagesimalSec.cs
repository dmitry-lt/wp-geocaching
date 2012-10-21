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

namespace GeocachingPlus.Model.Utils
{
    public class SexagesimalSec
    {
        public int degrees;
        public int minutes;
        public double seconds;

        public SexagesimalSec(SexagesimalSec sexagesimalSec)
        {
            this.degrees = sexagesimalSec.degrees;
            this.minutes = sexagesimalSec.minutes;
            this.seconds = sexagesimalSec.seconds;
        }

        public SexagesimalSec(int degrees, int minutes, double seconds)
        {
            //            if (Math.abs(degrees) > 180 || minutes >= 60 || seconds >= 60) {
            this.degrees = degrees;
            this.minutes = minutes;
            this.seconds = seconds;
        }

        public SexagesimalSec(double coordinate)
        {
            degrees = (int)coordinate;
            double fractoinMinutes = (coordinate - degrees) * 60;
            minutes = (int)(fractoinMinutes);
            seconds = (fractoinMinutes - minutes) * 60;
        }

        public SexagesimalSec roundTo(int i)
        {
            double precision = Math.Pow(10, i);
            SexagesimalSec copy = new SexagesimalSec(this);
            copy.seconds = Math.Round(seconds * precision);
            if (copy.seconds == 60 * precision)
            {
                copy.seconds = 0;
                copy.minutes++;
            }
            copy.seconds /= precision;
            if (copy.minutes == 60)
            {
                copy.minutes = 0;
                if (copy.degrees < 0)
                {
                    copy.degrees--;
                }
                else
                {
                    copy.degrees++;
                }
            }
            return copy;
        }

        public double toCoordinate()
        {
            return (degrees + (minutes / 60.0) + (seconds / 3600.0));
        }
    }
}
