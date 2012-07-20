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
        set
        {
            degrees = Convert.ToInt32(value);
            positive = degrees > 0 ? true : false;
        }
    }

    public string Minutes
    {
        get
        {
            return minutes.ToString();
        }
        set
        {
            minutes = Convert.ToInt32(value);
        }
    }

    public string MinutesFraction
    {
        get
        {
            return String.Format("{0:0.000}", minutesFraction).Substring(2);
        }
        set
        {
            string val = "0." + value;
            minutesFraction = Convert.ToDouble(val);
        }
    }

    public MinCoordinateViewModel(double coordinate)
    {
        positive = coordinate > 0 ? true : false;
        degrees = (int)coordinate;
        minutes = Math.Abs((int)((coordinate - degrees) * 60));
        minutesFraction = Math.Round(Math.Abs((coordinate - degrees)) * 60 - minutes, 3);
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