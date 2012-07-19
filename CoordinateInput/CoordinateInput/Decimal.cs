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

public class Decimal
{
    private int degrees;
    private double degreesFraction;

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

    public double DegreesFraction
    {
        get
        {
            return degreesFraction;
        }
        set
        {
            degreesFraction = value;
        }
    }

    public Decimal(double coordinate)
    {
        degrees = (int)coordinate;
        degreesFraction = Math.Round(coordinate - (int)coordinate, 6);
    }

    public double ToCoordinate()
    {
        return degrees + degreesFraction;
    }
}