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

namespace CoordinateInput
{
    public enum CoordinateType { Lat, Lng };

    public class CheckpointViewModel : BaseViewModel
    {
        private GeoCoordinate currentInputPointLocation;

        private bool latDegreesValid;
        private bool latMinutesValid;
        private bool latMinutesFractionValid;
        private bool dLatDegreesValid;
        private bool dLatDegreesFractionValid;
        private bool sLatDegreesValid;
        private bool sLatMinutesValid;
        private bool sLatSecondsValid;
        private bool sLatSecondsFractionValid;
        private bool lngDegreesValid;
        private bool lngMinutesValid;
        private bool lngMinutesFractionValid;
        private bool dLngDegreesValid;
        private bool dLngDegreesFractionValid;
        private bool sLngDegreesValid;
        private bool sLngMinutesValid;
        private bool sLngSecondsValid;
        private bool sLngSecondsFractionValid;

        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyPropertyChanged("Name");
            }
        }

        public bool LatDegreesValid
        {
            get
            {
                return latDegreesValid;
            }
            set
            {
                latDegreesValid = value;
                NotifyPropertyChanged("LatDegreesValid");
            }
        }

        public bool LatMinutesValid
        {
            get
            {
                return latMinutesValid;
            }
            set
            {
                latMinutesValid = value;
                NotifyPropertyChanged("LatMinutesValid");
            }
        }

        public bool LatMinutesFractionValid
        {
            get
            {
                return latMinutesFractionValid;
            }
            set
            {
                latMinutesFractionValid = value;
                NotifyPropertyChanged("LatMinutesFractionValid");
            }
        }

        public bool DLatDegreesValid
        {
            get
            {
                return dLatDegreesValid;
            }
            set
            {
                dLatDegreesValid = value;
                NotifyPropertyChanged("DLatDegreesValid");
            }
        }

        public bool DLatDegreesFractionValid
        {
            get
            {
                return dLatDegreesFractionValid;
            }
            set
            {
                dLatDegreesFractionValid = value;
                NotifyPropertyChanged("DLatDegreesFractionValid");
            }
        }

        public bool SLatDegreesValid
        {
            get
            {
                return sLatDegreesValid;
            }
            set
            {
                sLatDegreesValid = value;
                NotifyPropertyChanged("SLatDegreesValid");
            }
        }

        public bool SLatMinutesValid
        {
            get
            {
                return sLatMinutesValid;
            }
            set
            {
                sLatMinutesValid = value;
                NotifyPropertyChanged("SLatMinutesValid");
            }
        }

        public bool SLatSecondsValid
        {
            get
            {
                return sLatSecondsValid;
            }
            set
            {
                sLatSecondsValid = value;
                NotifyPropertyChanged("SLatSecondsValid");
            }
        }

        public bool SLatSecondsFractionValid
        {
            get
            {
                return sLatSecondsFractionValid;
            }
            set
            {
                sLatSecondsFractionValid = value;
                NotifyPropertyChanged("SLatSecondsFractionValid");
            }
        }

        public bool LngDegreesValid
        {
            get
            {
                return lngDegreesValid;
            }
            set
            {
                lngDegreesValid = value;
                NotifyPropertyChanged("LngDegreesValid");
            }
        }

        public bool LngMinutesValid
        {
            get
            {
                return lngMinutesValid;
            }
            set
            {
                lngMinutesValid = value;
                NotifyPropertyChanged("LngMinutesValid");
            }
        }

        public bool LngMinutesFractionValid
        {
            get
            {
                return lngMinutesFractionValid;
            }
            set
            {
                lngMinutesFractionValid = value;
                NotifyPropertyChanged("LngMinutesFractionValid");
            }
        }

        public bool DLngDegreesValid
        {
            get
            {
                return dLngDegreesValid;
            }
            set
            {
                dLngDegreesValid = value;
                NotifyPropertyChanged("DLngDegreesValid");
            }
        }

        public bool DLngDegreesFractionValid
        {
            get
            {
                return dLngDegreesFractionValid;
            }
            set
            {
                dLngDegreesFractionValid = value;
                NotifyPropertyChanged("DLngDegreesFractionValid");
            }
        }

        public bool SLngDegreesValid
        {
            get
            {
                return sLngDegreesValid;
            }
            set
            {
                sLngDegreesValid = value;
                NotifyPropertyChanged("SLngDegreesValid");
            }
        }

        public bool SLngMinutesValid
        {
            get
            {
                return sLngMinutesValid;
            }
            set
            {
                sLngMinutesValid = value;
                NotifyPropertyChanged("SLngMinutesValid");
            }
        }

        public bool SLngSecondsValid
        {
            get
            {
                return sLngSecondsValid;
            }
            set
            {
                sLngSecondsValid = value;
                NotifyPropertyChanged("SLngSecondsValid");
            }
        }

        public bool SLngSecondsFractionValid
        {
            get
            {
                return sLngSecondsFractionValid;
            }
            set
            {
                sLngSecondsFractionValid = value;
                NotifyPropertyChanged("SLngSecondsFractionValid");
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
                MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(currentInputPointLocation.Latitude, CoordinateType.Lat);
                LatDegreesValid = true;
                return minCoordinateViewModel.Degrees;
            }
            set
            {
                MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(currentInputPointLocation.Latitude, CoordinateType.Lat);

                if (minCoordinateViewModel.Degrees != value)
                {
                    if (LatDegreesValid = minCoordinateViewModel.SetDegrees(value))
                    {
                        currentInputPointLocation.Latitude = minCoordinateViewModel.ToCoordinate();
                    }
                }
                else
                {
                    LatDegreesValid = true;
                }
            }
        }

        public string LatMinutes
        {
            get
            {
                MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(currentInputPointLocation.Latitude, CoordinateType.Lat);
                LatMinutesValid = true;
                return minCoordinateViewModel.Minutes;
            }
            set
            {
                MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(currentInputPointLocation.Latitude, CoordinateType.Lat);

                if (minCoordinateViewModel.Minutes != value)
                {
                    if (LatMinutesValid = minCoordinateViewModel.SetMinutes(value))
                    {
                        currentInputPointLocation.Latitude = minCoordinateViewModel.ToCoordinate();
                    }
                }
                else
                {
                    LatMinutesValid = true;
                }
            }
        }

        public string LatMinutesFraction
        {
            get
            {
                MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(currentInputPointLocation.Latitude, CoordinateType.Lat);
                LatMinutesFractionValid = true;
                return minCoordinateViewModel.MinutesFraction;
            }
            set
            {
                MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(currentInputPointLocation.Latitude, CoordinateType.Lat);

                if (minCoordinateViewModel.MinutesFraction != value)
                {
                    if (LatMinutesFractionValid = minCoordinateViewModel.SetMinutesFraction(value))
                    {
                        currentInputPointLocation.Latitude = minCoordinateViewModel.ToCoordinate();
                    }
                }
                else
                {
                    LatMinutesFractionValid = true;
                }
            }
        }

        public string DLatDegrees
        {
            get
            {
                DegCoordinateViewModel degCoordinateViewModel = new DegCoordinateViewModel(currentInputPointLocation.Latitude, CoordinateType.Lat);
                DLatDegreesValid = true;
                return degCoordinateViewModel.Degrees;
            }
            set
            {
                DegCoordinateViewModel degCoordinateViewModel = new DegCoordinateViewModel(currentInputPointLocation.Latitude, CoordinateType.Lat);

                if (degCoordinateViewModel.Degrees != value)
                {
                    if (DLatDegreesValid = degCoordinateViewModel.SetDegrees(value))
                    {
                        currentInputPointLocation.Latitude = degCoordinateViewModel.ToCoordinate();
                    }
                }
                else
                {
                    DLatDegreesValid = true;
                }
            }
        }

        public string DLatDegreesFraction
        {
            get
            {
                DegCoordinateViewModel degCoordinateViewModel = new DegCoordinateViewModel(currentInputPointLocation.Latitude, CoordinateType.Lat);
                DLatDegreesFractionValid = true;
                return degCoordinateViewModel.DegreesFraction;
            }
            set
            {
                DegCoordinateViewModel degCoordinateViewModel = new DegCoordinateViewModel(currentInputPointLocation.Latitude, CoordinateType.Lat);

                if (degCoordinateViewModel.DegreesFraction != value)
                {
                    if (DLatDegreesFractionValid = degCoordinateViewModel.SetDegreesFraction(value))
                    {
                        currentInputPointLocation.Latitude = degCoordinateViewModel.ToCoordinate();
                    }
                }
                else
                {
                    DLatDegreesFractionValid = true;
                }
            }
        }

        public string SLatDegrees
        {
            get
            {
                SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(currentInputPointLocation.Latitude, CoordinateType.Lat);
                SLatDegreesValid = true;
                return secCoordinateViewModel.Degrees;
            }
            set
            {
                SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(currentInputPointLocation.Latitude, CoordinateType.Lat);

                if (secCoordinateViewModel.Degrees != value)
                {
                    if (SLatDegreesValid = secCoordinateViewModel.SetDegrees(value))
                    {
                        currentInputPointLocation.Latitude = secCoordinateViewModel.ToCoordinate();
                    }
                }
                else
                {
                    SLatDegreesValid = true;
                }
            }
        }

        public string SLatMinutes
        {
            get
            {
                SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(currentInputPointLocation.Latitude, CoordinateType.Lat);
                SLatMinutesValid = true;
                return secCoordinateViewModel.Minutes;
            }
            set
            {
                SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(currentInputPointLocation.Latitude, CoordinateType.Lat);

                if (secCoordinateViewModel.Minutes != value)
                {
                    if (SLatMinutesValid = secCoordinateViewModel.SetMinutes(value))
                    {
                        currentInputPointLocation.Latitude = secCoordinateViewModel.ToCoordinate();
                    }
                }
                else
                {
                    SLatMinutesValid = true;
                }
            }
        }

        public string SLatSeconds
        {
            get
            {
                SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(currentInputPointLocation.Latitude, CoordinateType.Lat);
                SLatSecondsValid = true;
                return secCoordinateViewModel.Seconds;
            }
            set
            {
                SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(currentInputPointLocation.Latitude, CoordinateType.Lat);

                if (secCoordinateViewModel.Seconds != value)
                {
                    if (SLatSecondsValid = secCoordinateViewModel.SetSeconds(value))
                    {
                        currentInputPointLocation.Latitude = secCoordinateViewModel.ToCoordinate();
                    }
                }
                else
                {
                    SLatSecondsValid = true;
                }
            }
        }

        public string SLatSecondsFraction
        {
            get
            {
                SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(currentInputPointLocation.Latitude, CoordinateType.Lat);
                SLatSecondsFractionValid = true;
                return secCoordinateViewModel.SecondsFraction;
            }
            set
            {
                SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(currentInputPointLocation.Latitude, CoordinateType.Lat);

                if (secCoordinateViewModel.SecondsFraction != value)
                {
                    if (SLatSecondsFractionValid = secCoordinateViewModel.SetSecondsFraction(value))
                    {
                        currentInputPointLocation.Latitude = secCoordinateViewModel.ToCoordinate();
                    }
                }
                else
                {
                    SLatSecondsFractionValid = true;
                }
            }
        }

        public string LngDegrees
        {
            get
            {
                MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(currentInputPointLocation.Longitude, CoordinateType.Lng);
                LngDegreesValid = true;
                return minCoordinateViewModel.Degrees;
            }
            set
            {
                MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(currentInputPointLocation.Longitude, CoordinateType.Lng);

                if (minCoordinateViewModel.Degrees != value)
                {
                    if (LngDegreesValid = minCoordinateViewModel.SetDegrees(value))
                    {
                        currentInputPointLocation.Longitude = minCoordinateViewModel.ToCoordinate();
                    }
                }
                else
                {
                    LngDegreesValid = true;
                }
            }
        }

        public string LngMinutes
        {
            get
            {
                MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(currentInputPointLocation.Longitude, CoordinateType.Lng);
                LngMinutesValid = true;
                return minCoordinateViewModel.Minutes;
            }
            set
            {
                MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(currentInputPointLocation.Longitude, CoordinateType.Lng);

                if (minCoordinateViewModel.Minutes != value)
                {
                    if (LngMinutesValid = minCoordinateViewModel.SetMinutes(value))
                    {
                        currentInputPointLocation.Longitude = minCoordinateViewModel.ToCoordinate();
                    }
                }
                else
                {
                    LngMinutesValid = true;
                }
            }
        }

        public string LngMinutesFraction
        {
            get
            {
                MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(currentInputPointLocation.Longitude, CoordinateType.Lng);
                LngMinutesFractionValid = true;
                return minCoordinateViewModel.MinutesFraction;
            }
            set
            {
                MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(currentInputPointLocation.Longitude, CoordinateType.Lng);

                if (minCoordinateViewModel.MinutesFraction != value)
                {
                    if (LngMinutesFractionValid = minCoordinateViewModel.SetMinutesFraction(value))
                    {
                        currentInputPointLocation.Longitude = minCoordinateViewModel.ToCoordinate();
                    }
                }
                else
                {
                    LngMinutesFractionValid = true;
                }
            }
        }

        public string DLngDegrees
        {
            get
            {
                DegCoordinateViewModel degCoordinateViewModel = new DegCoordinateViewModel(currentInputPointLocation.Longitude, CoordinateType.Lng);
                DLngDegreesValid = true;
                return degCoordinateViewModel.Degrees;
            }
            set
            {
                DegCoordinateViewModel degCoordinateViewModel = new DegCoordinateViewModel(currentInputPointLocation.Longitude, CoordinateType.Lng);

                if (degCoordinateViewModel.Degrees != value)
                {
                    if (DLngDegreesValid = degCoordinateViewModel.SetDegrees(value))
                    {
                        currentInputPointLocation.Longitude = degCoordinateViewModel.ToCoordinate();
                    }
                }
                else
                {
                    DLngDegreesValid = true;
                }
            }
        }

        public string DLngDegreesFraction
        {
            get
            {
                DegCoordinateViewModel degCoordinateViewModel = new DegCoordinateViewModel(currentInputPointLocation.Longitude, CoordinateType.Lng);
                DLngDegreesFractionValid = true;
                return degCoordinateViewModel.DegreesFraction;
            }
            set
            {
                DegCoordinateViewModel degCoordinateViewModel = new DegCoordinateViewModel(currentInputPointLocation.Longitude, CoordinateType.Lng);

                if (degCoordinateViewModel.DegreesFraction != value)
                {
                    if (DLngDegreesFractionValid = degCoordinateViewModel.SetDegreesFraction(value))
                    {
                        currentInputPointLocation.Longitude = degCoordinateViewModel.ToCoordinate();
                    }
                }
                else
                {
                    DLngDegreesFractionValid = true;
                }
            }
        }

        public string SLngDegrees
        {
            get
            {
                SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(currentInputPointLocation.Longitude, CoordinateType.Lng);
                SLngDegreesValid = true;
                return secCoordinateViewModel.Degrees;
            }
            set
            {
                SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(currentInputPointLocation.Longitude, CoordinateType.Lng);

                if (secCoordinateViewModel.Degrees != value)
                {
                    if (SLngDegreesValid = secCoordinateViewModel.SetDegrees(value))
                    {
                        currentInputPointLocation.Longitude = secCoordinateViewModel.ToCoordinate();
                    }
                }
                else
                {
                    SLngDegreesValid = true;
                }
            }
        }

        public string SLngMinutes
        {
            get
            {
                SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(currentInputPointLocation.Longitude, CoordinateType.Lng);
                SLngMinutesValid = true;
                return secCoordinateViewModel.Minutes;
            }
            set
            {
                SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(currentInputPointLocation.Longitude, CoordinateType.Lng);

                if (secCoordinateViewModel.Minutes != value)
                {
                    if (SLngMinutesValid = secCoordinateViewModel.SetMinutes(value))
                    {
                        currentInputPointLocation.Longitude = secCoordinateViewModel.ToCoordinate();
                    }
                }
                else
                {
                    SLngMinutesValid = true;
                }
            }
        }

        public string SLngSeconds
        {
            get
            {
                SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(currentInputPointLocation.Longitude, CoordinateType.Lng);
                SLngSecondsValid = true;
                return secCoordinateViewModel.Seconds;
            }
            set
            {
                SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(currentInputPointLocation.Longitude, CoordinateType.Lng);

                if (secCoordinateViewModel.Seconds != value)
                {
                    if (SLngSecondsValid = secCoordinateViewModel.SetSeconds(value))
                    {
                        currentInputPointLocation.Longitude = secCoordinateViewModel.ToCoordinate();
                    }
                }
                else
                {
                    SLngSecondsValid = true;
                }
            }
        }

        public string SLngSecondsFraction
        {
            get
            {
                SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(currentInputPointLocation.Longitude, CoordinateType.Lng);
                SLngSecondsFractionValid = true;
                return secCoordinateViewModel.SecondsFraction;
            }
            set
            {
                SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(currentInputPointLocation.Longitude, CoordinateType.Lng);

                if (secCoordinateViewModel.SecondsFraction != value)
                {
                    if (SLngSecondsFractionValid = secCoordinateViewModel.SetSecondsFraction(value))
                    {
                        currentInputPointLocation.Longitude = secCoordinateViewModel.ToCoordinate();
                    }
                }
                else
                {
                    SLngSecondsFractionValid = true;
                }
            }
        }

        public CheckpointViewModel()
        {
            currentInputPointLocation = new GeoCoordinate(59.898580584384277, 30.285182952880845);
            LatDegreesValid = true;
            LatMinutesValid = true;
            LatMinutesFractionValid = true;
            DLatDegreesValid = true;
            DLatDegreesFractionValid = true;
            SLatDegreesValid = true;
            SLatMinutesValid = true;
            SLatSecondsValid = true;
            SLatSecondsFractionValid = true;
            LngDegreesValid = true;
            LngMinutesValid = true;
            LngMinutesFractionValid = true;
            DLngDegreesValid = true;
            DLngDegreesFractionValid = true;
            SLngDegreesValid = true;
            SLngMinutesValid = true;
            SLngSecondsValid = true;
            SLngSecondsFractionValid = true;
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
            NotifyPropertyChanged("SLatSecondsFraction");
            NotifyPropertyChanged("DLngDegrees");
            NotifyPropertyChanged("DLngDegreesFraction");
            NotifyPropertyChanged("LngDegrees");
            NotifyPropertyChanged("LngMinutes");
            NotifyPropertyChanged("LngMinutesFraction");
            NotifyPropertyChanged("SLngDegrees");
            NotifyPropertyChanged("SLngMinutes");
            NotifyPropertyChanged("SLngSeconds");
            NotifyPropertyChanged("SLngSecondsFraction");
        }
    }
}