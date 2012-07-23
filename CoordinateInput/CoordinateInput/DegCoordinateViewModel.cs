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
    private const string FormatDegreesFraction = "{0:0.000000}";
    private const string DotPosition = "0.";
    private const int RoundDegreesFraction = 6;

    private int degrees;
    private double degreesFraction;
    private bool positive;

    public string Degrees
    {
        get
        {
            return degrees.ToString();
        }
    }

    public string DegreesFraction
    {
        get
        {
            return String.Format(FormatDegreesFraction, degreesFraction).Substring(2);
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

    public bool SetDegreesFraction(string value)
    {
        string val = DotPosition + value;
        double degFraction;

        if (double.TryParse(val, out degFraction))
        {
            degreesFraction = degFraction;
            return true;
        }

        return false;
    }

    public DegCoordinateViewModel(double coordinate)
    {
        positive = coordinate > 0 ? true : false;
        degrees = (int)coordinate;
        degreesFraction = Math.Round(Math.Abs(coordinate - (int)coordinate), RoundDegreesFraction);
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