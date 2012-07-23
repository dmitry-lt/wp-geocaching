﻿using System;
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

    private Visibility latDegreesValidate;
    private Visibility latMinutesValidate;
    private Visibility latMinutesFractionValidate;
    private Visibility dLatDegreesValidate;
    private Visibility dLatDegreesFractionValidate;
    private Visibility sLatDegreesValidate;
    private Visibility sLatMinutesValidate;
    private Visibility sLatSecondsValidate;

    public Visibility LatDegreesValidate
    {
        get
        {
            return latDegreesValidate;
        }
        set
        {
            latDegreesValidate = value;
            NotifyPropertyChanged("LatDegreesValidate");
        }
    }

    public Visibility LatMinutesValidate
    {
        get
        {
            return latMinutesValidate;
        }
        set
        {
            latMinutesValidate = value;
            NotifyPropertyChanged("LatMinutesValidate");
        }
    }

    public Visibility LatMinutesFractionValidate
    {
        get
        {
            return latMinutesFractionValidate;
        }
        set
        {
            latMinutesFractionValidate = value;
            NotifyPropertyChanged("LatMinutesFractionValidate");
        }
    }

    public Visibility DLatDegreesValidate
    {
        get
        {
            return dLatDegreesValidate;
        }
        set
        {
            dLatDegreesValidate = value;
            NotifyPropertyChanged("DLatDegreesValidate");
        }
    }

    public Visibility DLatDegreesFractionValidate
    {
        get
        {
            return dLatDegreesFractionValidate;
        }
        set
        {
            dLatDegreesFractionValidate = value;
            NotifyPropertyChanged("DLatDegreesFractionValidate");
        }
    }

    public Visibility SLatDegreesValidate
    {
        get
        {
            return sLatDegreesValidate;
        }
        set
        {
            sLatDegreesValidate = value;
            NotifyPropertyChanged("SLatDegreesValidate");
        }
    }

    public Visibility SLatMinutesValidate
    {
        get
        {
            return sLatMinutesValidate;
        }
        set
        {
            sLatMinutesValidate = value;
            NotifyPropertyChanged("SLatMinutesValidate");
        }
    }

    public Visibility SLatSecondsValidate
    {
        get
        {
            return sLatSecondsValidate;
        }
        set
        {
            sLatSecondsValidate = value;
            NotifyPropertyChanged("SLatSecondsValidate");
        }
    }

    public GeoCoordinate CurrentInputPointLocation
    {
        get
        {
            return currentInputPointLocation;
        }
    }

    public string LatDegrees
    {
        get
        {
            MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(currentInputPointLocation.Latitude);
            LatDegreesValidate = Visibility.Collapsed;
            return minCoordinateViewModel.Degrees;
        }
        set
        {
            MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(currentInputPointLocation.Latitude);

            if (minCoordinateViewModel.Degrees != value)
            {
                if (minCoordinateViewModel.SetDegrees(value))
                {
                    currentInputPointLocation.Latitude = minCoordinateViewModel.ToCoordinate();
                    LatDegreesValidate = Visibility.Collapsed;
                }
                else
                {
                    LatDegreesValidate = Visibility.Visible;
                }
            }
            else
            {
                LatDegreesValidate = Visibility.Collapsed;
            }
        }
    }

    public string LatMinutes
    {
        get
        {
            MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(currentInputPointLocation.Latitude);
            LatMinutesValidate = Visibility.Collapsed;
            return minCoordinateViewModel.Minutes;
        }
        set
        {
            MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(currentInputPointLocation.Latitude);

            if (minCoordinateViewModel.Minutes != value)
            {
                if (minCoordinateViewModel.SetMinutes(value))
                {
                    currentInputPointLocation.Latitude = minCoordinateViewModel.ToCoordinate();
                    LatMinutesValidate = Visibility.Collapsed;
                }
                else
                {
                    LatMinutesValidate = Visibility.Visible;
                }
            }
            else
            {
                LatMinutesValidate = Visibility.Collapsed;
            }
        }
    }

    public string LatMinutesFraction
    {
        get
        {
            MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(currentInputPointLocation.Latitude);
            LatMinutesFractionValidate = Visibility.Collapsed;
            return minCoordinateViewModel.MinutesFraction;
        }
        set
        {
            MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(currentInputPointLocation.Latitude);

            if (minCoordinateViewModel.MinutesFraction != value)
            {
                if (minCoordinateViewModel.SetMinutesFraction(value))
                {
                    currentInputPointLocation.Latitude = minCoordinateViewModel.ToCoordinate();
                    LatMinutesFractionValidate = Visibility.Collapsed;
                }
                else
                {
                    LatMinutesFractionValidate = Visibility.Visible;
                }
            }
            else
            {
                LatMinutesFractionValidate = Visibility.Collapsed;
            }
        }
    }

    public string DLatDegrees
    {
        get
        {
            DegCoordinateViewModel degCoordinateViewModel = new DegCoordinateViewModel(currentInputPointLocation.Latitude);
            DLatDegreesValidate = Visibility.Collapsed;
            return degCoordinateViewModel.Degrees;
        }
        set
        {
            DegCoordinateViewModel degCoordinateViewModel = new DegCoordinateViewModel(currentInputPointLocation.Latitude);

            if (degCoordinateViewModel.Degrees != value)
            {
                if (degCoordinateViewModel.SetDegrees(value))
                {
                    currentInputPointLocation.Latitude = degCoordinateViewModel.ToCoordinate();
                    DLatDegreesValidate = Visibility.Collapsed;
                }
                else
                {
                    DLatDegreesValidate = Visibility.Visible;
                }
            }
            else
            {
                DLatDegreesValidate = Visibility.Collapsed;
            }
        }
    }

    public string DLatDegreesFraction
    {
        get
        {
            DegCoordinateViewModel degCoordinateViewModel = new DegCoordinateViewModel(currentInputPointLocation.Latitude);
            DLatDegreesFractionValidate = Visibility.Collapsed;
            return degCoordinateViewModel.DegreesFraction;
        }
        set
        {
            DegCoordinateViewModel degCoordinateViewModel = new DegCoordinateViewModel(currentInputPointLocation.Latitude);

            if (degCoordinateViewModel.DegreesFraction != value)
            {
                if (degCoordinateViewModel.SetDegreesFraction(value))
                {
                    currentInputPointLocation.Latitude = degCoordinateViewModel.ToCoordinate();
                    DLatDegreesFractionValidate = Visibility.Collapsed;
                }
                else
                {
                    DLatDegreesFractionValidate = Visibility.Visible;
                }
            }
            else
            {
                DLatDegreesFractionValidate = Visibility.Collapsed;
            }
        }
    }

    public string SLatDegrees
    {
        get
        {
            SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(currentInputPointLocation.Latitude);
            SLatDegreesValidate = Visibility.Collapsed;
            return secCoordinateViewModel.Degrees;
        }
        set
        {
            SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(currentInputPointLocation.Latitude);

            if (secCoordinateViewModel.Degrees != value)
            {
                if (secCoordinateViewModel.SetDegrees(value))
                {
                    currentInputPointLocation.Latitude = secCoordinateViewModel.ToCoordinate();
                    SLatDegreesValidate = Visibility.Collapsed;
                }
                else
                {
                    SLatDegreesValidate = Visibility.Visible;
                }
            }
            else
            {
                SLatDegreesValidate = Visibility.Collapsed;
            }
        }
    }

    public string SLatMinutes
    {
        get
        {
            SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(currentInputPointLocation.Latitude);
            SLatDegreesValidate = Visibility.Collapsed;
            return secCoordinateViewModel.Minutes;
        }
        set
        {
            SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(currentInputPointLocation.Latitude);

            if (secCoordinateViewModel.Minutes != value)
            {
                if (secCoordinateViewModel.SetMinutes(value))
                {
                    currentInputPointLocation.Latitude = secCoordinateViewModel.ToCoordinate();
                    SLatMinutesValidate = Visibility.Collapsed;
                }
                else
                {
                    SLatMinutesValidate = Visibility.Visible;
                }
            }
            else
            {
                SLatMinutesValidate = Visibility.Collapsed;
            }
        }
    }

    public string SLatSeconds
    {
        get
        {
            SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(currentInputPointLocation.Latitude);
            SLatDegreesValidate = Visibility.Collapsed;
            return secCoordinateViewModel.Seconds;
        }
        set
        {
            SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(currentInputPointLocation.Latitude);

            if (secCoordinateViewModel.Seconds != value)
            {
                if (secCoordinateViewModel.SetSeconds(value))
                {
                    currentInputPointLocation.Latitude = secCoordinateViewModel.ToCoordinate();
                    SLatSecondsValidate = Visibility.Collapsed;
                }
                else
                {
                    SLatSecondsValidate = Visibility.Visible;
                }
            }
            else
            {
                SLatSecondsValidate = Visibility.Collapsed;
            }
        }
    }

    public CheckpointViewModel()
    {
        currentInputPointLocation = new GeoCoordinate(/*59.898580584384277*/3.97, 30.285182952880845);
        LatDegreesValidate = Visibility.Collapsed;
        LatMinutesValidate = Visibility.Collapsed;
        LatMinutesFractionValidate = Visibility.Collapsed;
        DLatDegreesValidate = Visibility.Collapsed;
        DLatDegreesFractionValidate = Visibility.Collapsed;
        SLatDegreesValidate = Visibility.Collapsed;
        SLatMinutesValidate = Visibility.Collapsed;
        SLatSecondsValidate = Visibility.Collapsed;
    }

    public void Refresh()
    {
        NotifyPropertyChanged("DLatDegrees");
        NotifyPropertyChanged("DLatDegreesFraction");
        NotifyPropertyChanged("LatDegrees");
        NotifyPropertyChanged("LatMinutes");
        NotifyPropertyChanged("LatMinutesFraction");
        NotifyPropertyChanged("SLatDegrees");
        NotifyPropertyChanged("SLatMinutes");
        NotifyPropertyChanged("SLatSeconds");
    }
}