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
using System.ComponentModel;
using System.Device.Location;
using System.Globalization;

public class CreateCheckpointViewModel : BaseViewModel
{
    private GeoCoordinate currentInputPointLocation;

    public GeoCoordinate CurrentInputPointLocation
    {
        get
        {
            return this.currentInputPointLocation;
        }
    }

    public string LatDegrees
    {
        get
        {
            Sexagesimal sexagesimal = new Sexagesimal(currentInputPointLocation.Latitude);
            return sexagesimal.Degrees.ToString();
        }
        set
        {
            Sexagesimal sexagesimal = new Sexagesimal(currentInputPointLocation.Latitude);

            if (sexagesimal.Degrees != Convert.ToInt32(value))
            {
                sexagesimal.Degrees = Convert.ToInt32(value);
                currentInputPointLocation.Latitude = sexagesimal.ToCoordinate();
                NotifyPropertyChanged("DLatDegrees");
                NotifyPropertyChanged("DLatDegreesFraction");
            }
        }
    }

    public string LatMinutes
    {
        get
        {
            Sexagesimal sexagesimal = new Sexagesimal(currentInputPointLocation.Latitude);
            return sexagesimal.Minutes.ToString();
        }
        set
        {
            Sexagesimal sexagesimal = new Sexagesimal(currentInputPointLocation.Latitude);

            if (sexagesimal.Minutes != Convert.ToInt32(value))
            {
                sexagesimal.Minutes = Convert.ToInt32(value);
                currentInputPointLocation.Latitude = sexagesimal.ToCoordinate();
                NotifyPropertyChanged("DLatDegrees");
                NotifyPropertyChanged("DLatDegreesFraction");
            }
        }
    }

    public string LatMinutesFraction
    {
        get
        {
            Sexagesimal sexagesimal = new Sexagesimal(currentInputPointLocation.Latitude);
            return sexagesimal.MinutesFraction.ToString().Substring(2);
        }
        set
        {
            Sexagesimal sexagesimal = new Sexagesimal(currentInputPointLocation.Latitude);
            string val = "0." + value;

            if (sexagesimal.MinutesFraction != Convert.ToDouble(val))
            {
                sexagesimal.MinutesFraction = Convert.ToDouble(val);
                currentInputPointLocation.Latitude = sexagesimal.ToCoordinate();
                NotifyPropertyChanged("DLatDegrees");
                NotifyPropertyChanged("DLatDegreesFraction");
            }
        }
    }

    public string DLatDegrees
    {
        get
        {
            Decimal dec = new Decimal(currentInputPointLocation.Latitude);
            return dec.Degrees.ToString();
        }
        set
        {
            Decimal dec = new Decimal(currentInputPointLocation.Latitude);

            if (dec.Degrees != Convert.ToInt32(value))
            {
                dec.Degrees = Convert.ToInt32(value);
                currentInputPointLocation.Latitude = dec.ToCoordinate();
                NotifyPropertyChanged("LatDegrees");
                NotifyPropertyChanged("LatMinutes");
                NotifyPropertyChanged("LatMinutesFraction");
            }
        }
    }

    public string DLatDegreesFraction
    {
        get
        {
            Decimal dec = new Decimal(currentInputPointLocation.Latitude);
            return dec.DegreesFraction.ToString().Substring(2);
        }
        set
        {
            Decimal dec = new Decimal(currentInputPointLocation.Latitude);
            string val = "0." + value;

            if (dec.DegreesFraction != Convert.ToDouble(val))
            {
                dec.DegreesFraction = Convert.ToDouble(val);
                currentInputPointLocation.Latitude = dec.ToCoordinate();
                NotifyPropertyChanged("LatDegrees");
                NotifyPropertyChanged("LatMinutes");
                NotifyPropertyChanged("LatMinutesFraction");
            }
        }
    }

    public CreateCheckpointViewModel()
    {
        currentInputPointLocation = new GeoCoordinate(59.898580584384277, 30.285182952880845);
    }
}