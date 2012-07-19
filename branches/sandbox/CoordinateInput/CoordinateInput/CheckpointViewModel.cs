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

public class CheckpointViewModel : BaseViewModel
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
            MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(currentInputPointLocation.Latitude);
            return minCoordinateViewModel.Degrees;
        }
        set
        {
            MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(currentInputPointLocation.Latitude);

            if (minCoordinateViewModel.Degrees != value)
            {
                minCoordinateViewModel.Degrees = value;
                currentInputPointLocation.Latitude = minCoordinateViewModel.ToCoordinate();
            }
        }
    }

    public string LatMinutes
    {
        get
        {
            MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(currentInputPointLocation.Latitude);
            return minCoordinateViewModel.Minutes;
        }
        set
        {
            MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(currentInputPointLocation.Latitude);

            if (minCoordinateViewModel.Minutes != value)
            {
                minCoordinateViewModel.Minutes = value;
                currentInputPointLocation.Latitude = minCoordinateViewModel.ToCoordinate();
            }
        }
    }

    public string LatMinutesFraction
    {
        get
        {
            MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(currentInputPointLocation.Latitude);
            return minCoordinateViewModel.MinutesFraction;
        }
        set
        {
            MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(currentInputPointLocation.Latitude);

            if (minCoordinateViewModel.MinutesFraction != value)
            {
                minCoordinateViewModel.MinutesFraction = value;
                currentInputPointLocation.Latitude = minCoordinateViewModel.ToCoordinate();
            }
        }
    }

    public string DLatDegrees
    {
        get
        {
            DegCoordinateViewModel degCoordinateViewModel = new DegCoordinateViewModel(currentInputPointLocation.Latitude);
            return degCoordinateViewModel.Degrees;
        }
        set
        {
            DegCoordinateViewModel degCoordinateViewModel = new DegCoordinateViewModel(currentInputPointLocation.Latitude);

            if (degCoordinateViewModel.Degrees != value)
            {
                degCoordinateViewModel.Degrees = value;
                currentInputPointLocation.Latitude = degCoordinateViewModel.ToCoordinate();
            }
        }
    }

    public string DLatDegreesFraction
    {
        get
        {
            DegCoordinateViewModel degCoordinateViewModel = new DegCoordinateViewModel(currentInputPointLocation.Latitude);
            return degCoordinateViewModel.DegreesFraction;
        }
        set
        {
            DegCoordinateViewModel degCoordinateViewModel = new DegCoordinateViewModel(currentInputPointLocation.Latitude);

            if (degCoordinateViewModel.DegreesFraction != value)
            {
                degCoordinateViewModel.DegreesFraction = value;
                currentInputPointLocation.Latitude = degCoordinateViewModel.ToCoordinate();
            }
        }
    }

    public CheckpointViewModel()
    {
        currentInputPointLocation = new GeoCoordinate(59.898580584384277, 30.285182952880845);
    }

    public void Refresh()
    {
        NotifyPropertyChanged("DLatDegrees");
        NotifyPropertyChanged("DLatDegreesFraction");
        NotifyPropertyChanged("LatDegrees");
        NotifyPropertyChanged("LatMinutes");
        NotifyPropertyChanged("LatMinutesFraction");
    }
}