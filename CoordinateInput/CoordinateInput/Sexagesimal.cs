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

public class Sexagesimal
{
    private int degrees;
    private int minutes;
    private double minutesFraction;

    public int Degrees
    {
        get
        {
            return degrees;
        }
        set
        {
            degrees = value;
        }
    }

    public int Minutes
    {
        get
        {
            return minutes;
        }
        set
        {
            minutes = value;
        }
    }

    public double MinutesFraction
    {
        get
        {
            return minutesFraction;
        }
        set
        {
            minutesFraction = value;
        }
    }

    public Sexagesimal(double coordinate)
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