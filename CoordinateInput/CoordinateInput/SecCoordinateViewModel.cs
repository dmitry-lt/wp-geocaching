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

public class SecCoordinateViewModel
{
    private const string FormatSecondsFraction = "{0:0.000}";
    private const string DotPosition = "0.";
    private const int MinLatitude = -90;
    private const int MaxLatitude = 90;
    private const int MinLongitude = -180;
    private const int MaxLongitude = 180;

    private int degrees;
    private int minutes;
    private int seconds;
    private double secondsFraction;
    private bool positive;
    private CoordinateType coordinateType;

    public string Degrees
    {
        get
        {
            return degrees.ToString();
        }
    }

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
                    positive = degrees > 0 ? true : false;
                    return true;
                }
            }
            else
            {
                if (deg > MinLongitude && deg < MaxLongitude)
                {
                    degrees = deg;
                    positive = degrees > 0 ? true : false;
                    return true;
                }
            }
        }

        return false;
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

        if (double.TryParse(val, out secFraction) && val != DotPosition)
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
        double fractoinMinutes = Math.Abs(coordinate - degrees) * 60;
        minutes = (int)fractoinMinutes;
        seconds = (int)((fractoinMinutes - minutes) * 60);
        secondsFraction = ((fractoinMinutes - minutes) * 60) - seconds;

        if (1 - (fractoinMinutes - minutes) < 0.0000001)
        {
            minutes++;
            seconds = 0;
            secondsFraction = 0;
        }
    }

    public double ToCoordinate()
    {
        if (positive)
        {
            return (degrees + (minutes / 60.0 + (seconds + secondsFraction) / 3600.0));
        }

        return (degrees - (minutes / 60.0 + (seconds + secondsFraction) / 3600.0));
    }
}