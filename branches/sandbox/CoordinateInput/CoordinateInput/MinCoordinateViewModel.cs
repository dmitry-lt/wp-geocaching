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
    private const int MinLatitude = -90;
    private const int MaxLatitude = 90;
    private const int MinLongitude = -180;
    private const int MaxLongitude = 180;

    private int degrees;
    private int minutes;
    private double minutesFraction;
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

    public bool SetMinutesFraction(string value)
    {
        string val = DotPosition + value;
        double minFraction;

        if (double.TryParse(val, out minFraction) && val != DotPosition)
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

        if (1 - minutesFraction < 0.0000001)
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