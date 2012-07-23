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

public class MinCoordinateViewModel
{
    private const string FormatMinutesFraction = "{0:0.000}";
    private const string DotPosition = "0.";
    private const int RoundMinutesFraction = 3;

    private int degrees;
    private int minutes;
    private double minutesFraction;
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

    public string MinutesFraction
    {
        get
        {
            return String.Format(FormatMinutesFraction, minutesFraction).Substring(2);
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

    public bool SetMinutesFraction(string value)
    {
        string val = DotPosition + value;
        double minFraction;

        if (double.TryParse(val, out minFraction))
        {
            minutesFraction = minFraction;
            return true;
        }

        return false;
    }

    public MinCoordinateViewModel(double coordinate)
    {
        positive = coordinate > 0 ? true : false;
        degrees = (int)coordinate;
        minutes = Math.Abs((int)((coordinate - degrees) * 60));
        minutesFraction = Math.Round(Math.Abs((coordinate - degrees)) * 60 - minutes, RoundMinutesFraction);
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