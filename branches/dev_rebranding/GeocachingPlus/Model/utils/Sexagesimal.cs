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
    public class Sexagesimal
    {
        private int degrees;
        private double minutes;

        public int Degrees
        {
            get
            {
                return this.degrees;
            }
        }
        public double Minutes
        {
            get
            {
                return this.minutes;
            }        
        }

        public Sexagesimal(Sexagesimal sexagesimal)
        {
            this.degrees = sexagesimal.degrees;
            this.minutes = sexagesimal.minutes;
        }

        public Sexagesimal(int degrees, double minutes)
        {
            this.degrees = degrees;
            this.minutes = minutes;
        }

        public Sexagesimal(double coordinate)
        {
            degrees = (int)coordinate;
            minutes = (coordinate - degrees) * 60;
        }

        public Sexagesimal roundTo(int i)
        {
            double precision = Math.Pow(10, i);
            Sexagesimal copy = new Sexagesimal(this);
            copy.minutes = Math.Round(minutes * precision);
            if (copy.minutes == 60 * precision)
            {
                copy.minutes = 0;
                if (copy.degrees < 0) copy.degrees--;
                else copy.degrees++;
            }
            copy.minutes /= precision;
            return copy;
        }

        public double toCoordinate()
        {
            return (degrees + (minutes / 60.0));
        }
    }
}
