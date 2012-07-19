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

    public string DegreesFraction
    {
        get
        {
            return degreesFraction.ToString().Substring(2);
        }
        set
        {
            string val = "0." + value;
            degreesFraction = Convert.ToDouble(val);
        }
    }

    public DegCoordinateViewModel(double coordinate)
    {
        degrees = (int)coordinate;
        degreesFraction = Math.Round(coordinate - (int)coordinate, 6);
    }

    public double ToCoordinate()
    {
        return degrees + degreesFraction;
    }
}