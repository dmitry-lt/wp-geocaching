using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using WP_Geocaching.Model;
using WP_Geocaching.View.Compass;

namespace WP_Geocaching.ViewModel
{

    public class CompassPageViewModal : ICompassView, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly SmoothCompassManager smoothCompassManager;
        private double _direction;

        public double Direction
        {
            get { return _direction; }
            set
            {
                _direction = value;
                NotifyPropertyChanged("Direction");
            }
        }

        public CompassPageViewModal()
        {
            smoothCompassManager = new SmoothCompassManager(this);
        }

        private DateTime time;
        public void SetDirection(double direction)
        {
            Direction = -direction;
            Debug.WriteLine("fps " + 1000 / (DateTime.Now - time).Milliseconds);
            time = DateTime.Now;
        }

        public void Start()
        {
            smoothCompassManager.Start();
        }

        public void Stop()
        {
            smoothCompassManager.Stop();
        }

        // NotifyPropertyChanged will raise the PropertyChanged event, 
        // passing the source property that is being updated.
        private Dictionary<string, PropertyChangedEventArgs> eventDictionary = new Dictionary<string, PropertyChangedEventArgs>(); //TODO: wtf? check
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventArgs changedEvent;
            bool sucess = eventDictionary.TryGetValue(propertyName, out changedEvent);
            if (!sucess)
            {
                changedEvent = new PropertyChangedEventArgs(propertyName);
                eventDictionary.Add(propertyName, changedEvent);
            }

            if (PropertyChanged != null)
            {
                PropertyChanged(this, changedEvent);
            }
        }
    }
}
