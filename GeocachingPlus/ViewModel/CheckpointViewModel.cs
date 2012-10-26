using System;
using System.Device.Location;
using GeocachingPlus.Model.DataBase;
using GeocachingPlus.Model;
using GeocachingPlus.Resources.Localization;

namespace GeocachingPlus.ViewModel
{
    public enum CoordinateType { Lat, Lng };

    public class CheckpointViewModel : BaseViewModel
    {
        private GeoCoordinate _currentInputPointLocation;

        private bool _latDegreesValid;
        private bool _latMinutesValid;
        private bool _latMinutesFractionValid;
        private bool _dLatDegreesValid;
        private bool _dLatDegreesFractionValid;
        private bool _sLatDegreesValid;
        private bool _sLatMinutesValid;
        private bool _sLatSecondsValid;
        private bool _sLatSecondsFractionValid;
        private bool _lngDegreesValid;
        private bool _lngMinutesValid;
        private bool _lngMinutesFractionValid;
        private bool _dLngDegreesValid;
        private bool _dLngDegreesFractionValid;
        private bool _sLngDegreesValid;
        private bool _sLngMinutesValid;
        private bool _sLngSecondsValid;
        private bool _sLngSecondsFractionValid;

        private string _name;
        private readonly bool _newCheckpoint;
        private readonly int _checkpointId;

        public bool NewCheckpoint { get { return _newCheckpoint; } }
        public int CheckpointId { get { return _checkpointId; } }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        public bool LatDegreesValid
        {
            get
            {
                return _latDegreesValid;
            }
            set
            {
                _latDegreesValid = value;
                RaisePropertyChanged(() => LatDegreesValid);
            }
        }

        public bool LatMinutesValid
        {
            get
            {
                return _latMinutesValid;
            }
            set
            {
                _latMinutesValid = value;
                RaisePropertyChanged(() => LatMinutesValid);
            }
        }

        public bool LatMinutesFractionValid
        {
            get
            {
                return _latMinutesFractionValid;
            }
            set
            {
                _latMinutesFractionValid = value;
                RaisePropertyChanged(() => LatMinutesFractionValid);
            }
        }

        public bool DLatDegreesValid
        {
            get
            {
                return _dLatDegreesValid;
            }
            set
            {
                _dLatDegreesValid = value;
                RaisePropertyChanged(() => DLatDegreesValid);
            }
        }

        public bool DLatDegreesFractionValid
        {
            get
            {
                return _dLatDegreesFractionValid;
            }
            set
            {
                _dLatDegreesFractionValid = value;
                RaisePropertyChanged(() => DLatDegreesFractionValid);
            }
        }

        public bool SLatDegreesValid
        {
            get
            {
                return _sLatDegreesValid;
            }
            set
            {
                _sLatDegreesValid = value;
                RaisePropertyChanged(() => SLatDegreesValid);
            }
        }

        public bool SLatMinutesValid
        {
            get
            {
                return _sLatMinutesValid;
            }
            set
            {
                _sLatMinutesValid = value;
                RaisePropertyChanged(() => SLatMinutesValid);
            }
        }

        public bool SLatSecondsValid
        {
            get
            {
                return _sLatSecondsValid;
            }
            set
            {
                _sLatSecondsValid = value;
                RaisePropertyChanged(() => SLatSecondsValid);
            }
        }

        public bool SLatSecondsFractionValid
        {
            get
            {
                return _sLatSecondsFractionValid;
            }
            set
            {
                _sLatSecondsFractionValid = value;
                RaisePropertyChanged(() => SLatSecondsFractionValid);
            }
        }

        public bool LngDegreesValid
        {
            get
            {
                return _lngDegreesValid;
            }
            set
            {
                _lngDegreesValid = value;
                RaisePropertyChanged(() => LngDegreesValid);
            }
        }

        public bool LngMinutesValid
        {
            get
            {
                return _lngMinutesValid;
            }
            set
            {
                _lngMinutesValid = value;
                RaisePropertyChanged(() => LngMinutesValid);
            }
        }

        public bool LngMinutesFractionValid
        {
            get
            {
                return _lngMinutesFractionValid;
            }
            set
            {
                _lngMinutesFractionValid = value;
                RaisePropertyChanged(() => LngMinutesFractionValid);
            }
        }

        public bool DLngDegreesValid
        {
            get
            {
                return _dLngDegreesValid;
            }
            set
            {
                _dLngDegreesValid = value;
                RaisePropertyChanged(() => DLngDegreesValid);
            }
        }

        public bool DLngDegreesFractionValid
        {
            get
            {
                return _dLngDegreesFractionValid;
            }
            set
            {
                _dLngDegreesFractionValid = value;
                RaisePropertyChanged(() => DLngDegreesFractionValid);
            }
        }

        public bool SLngDegreesValid
        {
            get
            {
                return _sLngDegreesValid;
            }
            set
            {
                _sLngDegreesValid = value;
                RaisePropertyChanged(() => SLngDegreesValid);
            }
        }

        public bool SLngMinutesValid
        {
            get
            {
                return _sLngMinutesValid;
            }
            set
            {
                _sLngMinutesValid = value;
                RaisePropertyChanged(() => SLngMinutesValid);
            }
        }

        public bool SLngSecondsValid
        {
            get
            {
                return _sLngSecondsValid;
            }
            set
            {
                _sLngSecondsValid = value;
                RaisePropertyChanged(() => SLngSecondsValid);
            }
        }

        public bool SLngSecondsFractionValid
        {
            get
            {
                return _sLngSecondsFractionValid;
            }
            set
            {
                _sLngSecondsFractionValid = value;
                RaisePropertyChanged(() => SLngSecondsFractionValid);
            }
        }

        public GeoCoordinate CurrentInputPointLocation
        {
            get
            {
                return _currentInputPointLocation;
            }
        }

        public string LatDegrees
        {
            get
            {
                MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(_currentInputPointLocation.Latitude, CoordinateType.Lat);
                LatDegreesValid = true;
                return minCoordinateViewModel.Degrees;
            }
            set
            {
                MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(_currentInputPointLocation.Latitude, CoordinateType.Lat);

                if (minCoordinateViewModel.Degrees != value)
                {
                    if (LatDegreesValid = minCoordinateViewModel.SetDegrees(value))
                    {
                        _currentInputPointLocation.Latitude = minCoordinateViewModel.ToCoordinate();
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
                MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(_currentInputPointLocation.Latitude, CoordinateType.Lat);
                LatMinutesValid = true;
                return minCoordinateViewModel.Minutes;
            }
            set
            {
                MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(_currentInputPointLocation.Latitude, CoordinateType.Lat);

                if (minCoordinateViewModel.Minutes != value)
                {
                    if (LatMinutesValid = minCoordinateViewModel.SetMinutes(value))
                    {
                        _currentInputPointLocation.Latitude = minCoordinateViewModel.ToCoordinate();
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
                MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(_currentInputPointLocation.Latitude, CoordinateType.Lat);
                LatMinutesFractionValid = true;
                return minCoordinateViewModel.MinutesFraction;
            }
            set
            {
                MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(_currentInputPointLocation.Latitude, CoordinateType.Lat);

                if (minCoordinateViewModel.MinutesFraction != value)
                {
                    if (LatMinutesFractionValid = minCoordinateViewModel.SetMinutesFraction(value))
                    {
                        _currentInputPointLocation.Latitude = minCoordinateViewModel.ToCoordinate();
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
                DegCoordinateViewModel degCoordinateViewModel = new DegCoordinateViewModel(_currentInputPointLocation.Latitude, CoordinateType.Lat);
                DLatDegreesValid = true;
                return degCoordinateViewModel.Degrees;
            }
            set
            {
                DegCoordinateViewModel degCoordinateViewModel = new DegCoordinateViewModel(_currentInputPointLocation.Latitude, CoordinateType.Lat);

                if (degCoordinateViewModel.Degrees != value)
                {
                    if (DLatDegreesValid = degCoordinateViewModel.SetDegrees(value))
                    {
                        _currentInputPointLocation.Latitude = degCoordinateViewModel.ToCoordinate();
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
                DegCoordinateViewModel degCoordinateViewModel = new DegCoordinateViewModel(_currentInputPointLocation.Latitude, CoordinateType.Lat);
                DLatDegreesFractionValid = true;
                return degCoordinateViewModel.DegreesFraction;
            }
            set
            {
                DegCoordinateViewModel degCoordinateViewModel = new DegCoordinateViewModel(_currentInputPointLocation.Latitude, CoordinateType.Lat);

                if (degCoordinateViewModel.DegreesFraction != value)
                {
                    if (DLatDegreesFractionValid = degCoordinateViewModel.SetDegreesFraction(value))
                    {
                        _currentInputPointLocation.Latitude = degCoordinateViewModel.ToCoordinate();
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
                SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(_currentInputPointLocation.Latitude, CoordinateType.Lat);
                SLatDegreesValid = true;
                return secCoordinateViewModel.Degrees;
            }
            set
            {
                SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(_currentInputPointLocation.Latitude, CoordinateType.Lat);

                if (secCoordinateViewModel.Degrees != value)
                {
                    if (SLatDegreesValid = secCoordinateViewModel.SetDegrees(value))
                    {
                        _currentInputPointLocation.Latitude = secCoordinateViewModel.ToCoordinate();
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
                SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(_currentInputPointLocation.Latitude, CoordinateType.Lat);
                SLatMinutesValid = true;
                return secCoordinateViewModel.Minutes;
            }
            set
            {
                SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(_currentInputPointLocation.Latitude, CoordinateType.Lat);

                if (secCoordinateViewModel.Minutes != value)
                {
                    if (SLatMinutesValid = secCoordinateViewModel.SetMinutes(value))
                    {
                        _currentInputPointLocation.Latitude = secCoordinateViewModel.ToCoordinate();
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
                SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(_currentInputPointLocation.Latitude, CoordinateType.Lat);
                SLatSecondsValid = true;
                return secCoordinateViewModel.Seconds;
            }
            set
            {
                SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(_currentInputPointLocation.Latitude, CoordinateType.Lat);

                if (secCoordinateViewModel.Seconds != value)
                {
                    if (SLatSecondsValid = secCoordinateViewModel.SetSeconds(value))
                    {
                        _currentInputPointLocation.Latitude = secCoordinateViewModel.ToCoordinate();
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
                SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(_currentInputPointLocation.Latitude, CoordinateType.Lat);
                SLatSecondsFractionValid = true;
                return secCoordinateViewModel.SecondsFraction;
            }
            set
            {
                SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(_currentInputPointLocation.Latitude, CoordinateType.Lat);

                if (secCoordinateViewModel.SecondsFraction != value)
                {
                    if (SLatSecondsFractionValid = secCoordinateViewModel.SetSecondsFraction(value))
                    {
                        _currentInputPointLocation.Latitude = secCoordinateViewModel.ToCoordinate();
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
                MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(_currentInputPointLocation.Longitude, CoordinateType.Lng);
                LngDegreesValid = true;
                return minCoordinateViewModel.Degrees;
            }
            set
            {
                MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(_currentInputPointLocation.Longitude, CoordinateType.Lng);

                if (minCoordinateViewModel.Degrees != value)
                {
                    if (LngDegreesValid = minCoordinateViewModel.SetDegrees(value))
                    {
                        _currentInputPointLocation.Longitude = minCoordinateViewModel.ToCoordinate();
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
                MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(_currentInputPointLocation.Longitude, CoordinateType.Lng);
                LngMinutesValid = true;
                return minCoordinateViewModel.Minutes;
            }
            set
            {
                MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(_currentInputPointLocation.Longitude, CoordinateType.Lng);

                if (minCoordinateViewModel.Minutes != value)
                {
                    if (LngMinutesValid = minCoordinateViewModel.SetMinutes(value))
                    {
                        _currentInputPointLocation.Longitude = minCoordinateViewModel.ToCoordinate();
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
                MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(_currentInputPointLocation.Longitude, CoordinateType.Lng);
                LngMinutesFractionValid = true;
                return minCoordinateViewModel.MinutesFraction;
            }
            set
            {
                MinCoordinateViewModel minCoordinateViewModel = new MinCoordinateViewModel(_currentInputPointLocation.Longitude, CoordinateType.Lng);

                if (minCoordinateViewModel.MinutesFraction != value)
                {
                    if (LngMinutesFractionValid = minCoordinateViewModel.SetMinutesFraction(value))
                    {
                        _currentInputPointLocation.Longitude = minCoordinateViewModel.ToCoordinate();
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
                DegCoordinateViewModel degCoordinateViewModel = new DegCoordinateViewModel(_currentInputPointLocation.Longitude, CoordinateType.Lng);
                DLngDegreesValid = true;
                return degCoordinateViewModel.Degrees;
            }
            set
            {
                DegCoordinateViewModel degCoordinateViewModel = new DegCoordinateViewModel(_currentInputPointLocation.Longitude, CoordinateType.Lng);

                if (degCoordinateViewModel.Degrees != value)
                {
                    if (DLngDegreesValid = degCoordinateViewModel.SetDegrees(value))
                    {
                        _currentInputPointLocation.Longitude = degCoordinateViewModel.ToCoordinate();
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
                DegCoordinateViewModel degCoordinateViewModel = new DegCoordinateViewModel(_currentInputPointLocation.Longitude, CoordinateType.Lng);
                DLngDegreesFractionValid = true;
                return degCoordinateViewModel.DegreesFraction;
            }
            set
            {
                DegCoordinateViewModel degCoordinateViewModel = new DegCoordinateViewModel(_currentInputPointLocation.Longitude, CoordinateType.Lng);

                if (degCoordinateViewModel.DegreesFraction != value)
                {
                    if (DLngDegreesFractionValid = degCoordinateViewModel.SetDegreesFraction(value))
                    {
                        _currentInputPointLocation.Longitude = degCoordinateViewModel.ToCoordinate();
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
                SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(_currentInputPointLocation.Longitude, CoordinateType.Lng);
                SLngDegreesValid = true;
                return secCoordinateViewModel.Degrees;
            }
            set
            {
                SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(_currentInputPointLocation.Longitude, CoordinateType.Lng);

                if (secCoordinateViewModel.Degrees != value)
                {
                    if (SLngDegreesValid = secCoordinateViewModel.SetDegrees(value))
                    {
                        _currentInputPointLocation.Longitude = secCoordinateViewModel.ToCoordinate();
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
                SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(_currentInputPointLocation.Longitude, CoordinateType.Lng);
                SLngMinutesValid = true;
                return secCoordinateViewModel.Minutes;
            }
            set
            {
                SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(_currentInputPointLocation.Longitude, CoordinateType.Lng);

                if (secCoordinateViewModel.Minutes != value)
                {
                    if (SLngMinutesValid = secCoordinateViewModel.SetMinutes(value))
                    {
                        _currentInputPointLocation.Longitude = secCoordinateViewModel.ToCoordinate();
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
                SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(_currentInputPointLocation.Longitude, CoordinateType.Lng);
                SLngSecondsValid = true;
                return secCoordinateViewModel.Seconds;
            }
            set
            {
                SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(_currentInputPointLocation.Longitude, CoordinateType.Lng);

                if (secCoordinateViewModel.Seconds != value)
                {
                    if (SLngSecondsValid = secCoordinateViewModel.SetSeconds(value))
                    {
                        _currentInputPointLocation.Longitude = secCoordinateViewModel.ToCoordinate();
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
                SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(_currentInputPointLocation.Longitude, CoordinateType.Lng);
                SLngSecondsFractionValid = true;
                return secCoordinateViewModel.SecondsFraction;
            }
            set
            {
                SecCoordinateViewModel secCoordinateViewModel = new SecCoordinateViewModel(_currentInputPointLocation.Longitude, CoordinateType.Lng);

                if (secCoordinateViewModel.SecondsFraction != value)
                {
                    if (SLngSecondsFractionValid = secCoordinateViewModel.SetSecondsFraction(value))
                    {
                        _currentInputPointLocation.Longitude = secCoordinateViewModel.ToCoordinate();
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
            var db = new CacheDataBase();
            _checkpointId = db.GetMaxCheckpointId(MapManager.Instance.Cache) + 1;
            _newCheckpoint = true;
            _name = String.Format(AppResources.DefaultCheckpointName, _checkpointId);
            var location = MapManager.Instance.Cache.Location;
            InitLocation(location.Latitude, location.Longitude);
        }

        public CheckpointViewModel(int checkpointId)
        {
            this._checkpointId = checkpointId;
            var db = new CacheDataBase();
            var dbCheckpoint = db.GetCheckpointByCacheAndCheckpointId(MapManager.Instance.Cache, checkpointId);
            _name = dbCheckpoint.Name;
            InitLocation(dbCheckpoint.Latitude, dbCheckpoint.Longitude);
        }

        private void InitLocation(double latitude, double longitude)
        {
            _currentInputPointLocation = new GeoCoordinate(latitude, longitude);

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
            RaisePropertyChanged(() => DLatDegrees);
            RaisePropertyChanged(() => DLatDegreesFraction);
            RaisePropertyChanged(() => LatDegrees);
            RaisePropertyChanged(() => LatMinutes);
            RaisePropertyChanged(() => LatMinutesFraction);
            RaisePropertyChanged(() => SLatDegrees);
            RaisePropertyChanged(() => SLatMinutes);
            RaisePropertyChanged(() => SLatSeconds);
            RaisePropertyChanged(() => SLatSecondsFraction);
            RaisePropertyChanged(() => DLngDegrees);
            RaisePropertyChanged(() => DLngDegreesFraction);
            RaisePropertyChanged(() => LngDegrees);
            RaisePropertyChanged(() => LngMinutes);
            RaisePropertyChanged(() => LngMinutesFraction);
            RaisePropertyChanged(() => SLngDegrees);
            RaisePropertyChanged(() => SLngMinutes);
            RaisePropertyChanged(() => SLngSeconds);
            RaisePropertyChanged(() => SLngSecondsFraction);
        }

        public void SavePoint()
        {
            var db = new CacheDataBase();
            if (_newCheckpoint)
            {
                db.AddActiveCheckpoint(MapManager.Instance.Cache, Name, CurrentInputPointLocation.Latitude, CurrentInputPointLocation.Longitude);
            }
            else
            {
                db.UpdateCheckpoint(MapManager.Instance.Cache, _checkpointId, Name, CurrentInputPointLocation.Latitude, CurrentInputPointLocation.Longitude);
            }
        }
    }
}