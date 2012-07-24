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
    private const int MinLatitude = -90;
    private const int MaxLatitude = 90;
    private const int MinLongitude = -180;
    private const int MaxLongitude = 180;

    private int degrees;
    private double degreesFraction;
    private bool positive;
    private CoordinateType coordinateType;

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

    public bool SetDegreesFraction(string value)
    {
        string val = DotPosition + value;
        double degFraction;

        if (double.TryParse(val, out degFraction) && val != DotPosition)
        {
            degreesFraction = degFraction;
            return true;
        }

        return false;
    }

    public DegCoordinateViewModel(double coordinate, CoordinateType type)
    {
        coordinateType = type;
        positive = coordinate > 0 ? true : false;
        degrees = (int)coordinate;
        degreesFraction = Math.Abs(coordinate - (int)coordinate);
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