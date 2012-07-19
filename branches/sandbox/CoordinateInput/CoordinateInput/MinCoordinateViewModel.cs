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

    public string Degrees
    {
        get
        {
            return degrees.ToString();
        }
        set
        {
            degrees = Convert.ToInt32(value);
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
            return minutesFraction.ToString().Substring(2);
        }
        set
        {
            string val = "0." + value;
            minutesFraction = Convert.ToDouble(val);
        }
    }

    public MinCoordinateViewModel(double coordinate)
    {
        degrees = (int)coordinate;
        minutes = (int)((coordinate - degrees) * 60);
        minutesFraction = Math.Round((coordinate - degrees) * 60 - minutes, 3);
    }

    public double ToCoordinate()
    {
        return degrees + (minutes + minutesFraction) / 60.0;
    }
}