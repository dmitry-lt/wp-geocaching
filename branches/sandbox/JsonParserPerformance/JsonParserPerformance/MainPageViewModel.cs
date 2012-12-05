using System.ComponentModel;
using JsonParserPerformance.Annotations;

namespace JsonParserPerformance
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private int _count;
        public int Count
        {
            get { return _count; }
            set
            {
                _count = value; 
                OnPropertyChanged("Count");
            }
        }

        public MainPageViewModel()
        {
            Count = 10;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
