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
    private const string FormatSeconds = "{0:0.000}";

    private int degrees;
    private int minutes;
    private double seconds;
    private bool positive;

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
            return String.Format(FormatSeconds, seconds);
        }
    }

    public bool SetDegrees(string value)
    {
        int deg;

        if (int.TryParse(value, out deg))
        {
            if (deg > -90 && deg < 90)
            {
                degrees = deg;
                positive = degrees > 0 ? true : false;
                return true;
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
        double sec;

        if (double.TryParse(value, out sec))
        {
            if (sec >= 0 && sec < 60)
            {
                seconds = sec;
                return true;
            }
        }

        return false;
    }

    public SecCoordinateViewModel(double coordinate)
    {
        positive = coordinate > 0 ? true : false;
        degrees = (int)coordinate;
        double fractoinMinutes = Math.Abs(coordinate - degrees) * 60;
        minutes = (int)(fractoinMinutes);
        seconds = Math.Round((fractoinMinutes - minutes) * 60);
    }

    public double ToCoordinate()
    {
        if (positive)
        {
            return degrees + (minutes / 60.0 + seconds / 3600.0);
        }

        return degrees - (minutes / 60.0 + seconds / 3600.0);
    }
}