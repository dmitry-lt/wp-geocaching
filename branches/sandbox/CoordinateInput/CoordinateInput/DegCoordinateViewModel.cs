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

public class DegCoordinateViewModel
{
    private int degrees;
    private double degreesFraction;
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

    public string DegreesFraction
    {
        get
        {
            return String.Format("{0:0.000000}", degreesFraction).Substring(2);
        }
        set
        {
            string val = "0." + value;
            degreesFraction = Convert.ToDouble(val);
        }
    }

    public DegCoordinateViewModel(double coordinate)
    {
        positive = coordinate > 0 ? true : false;
        degrees = (int)coordinate;
        degreesFraction = Math.Round(Math.Abs(coordinate - (int)coordinate), 6);
    }

    public double ToCoordinate()
    {
        if (positive)
        {
            return degrees + degreesFraction;
        }
        return degrees - degreesFraction;
    }
}