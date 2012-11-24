using System.ComponentModel;
using GeocachingPlus.Model.Utils;

namespace GeocachingPlus.Model.Api
{
    public class RequestCounter : INotifyPropertyChanged
    {
        public static RequestCounter LiveMap = new RequestCounter();

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly object _lock = new object();

        private int _requestsSent;
        public int RequestsSent { get { return _requestsSent; } }
        
        private int _requestsSucceeded;
        public int RequestsSucceeded { get { return _requestsSucceeded; } }
        
        private int _requestsFailed;
        public int RequestsFailed { get { return _requestsFailed; } }

        private bool _isLoading;
        public bool IsLoading { get { return _isLoading; } }

        private RequestCounter()
        {
        }

        private void CheckLoading()
        {
            var isloading = _requestsSent > _requestsSucceeded + _requestsFailed;
            if (isloading != _isLoading)
            {
                _isLoading = isloading;
                PropertyChanged.Raise(() => IsLoading);
            }
        }

        public void RequestSent()
        {
            lock (_lock)
            {
                _requestsSent++;
                PropertyChanged.Raise(() => RequestsSent);
                CheckLoading();
            }
        }

        public void RequestSucceeded()
        {
            lock (_lock)
            {
                _requestsSucceeded++;
                PropertyChanged.Raise(() => RequestsSucceeded);
                CheckLoading();
            }
        }

        public void RequestFailed()
        {
            lock (_lock)
            {
                _requestsFailed++;
                PropertyChanged.Raise(() => RequestsFailed);
                CheckLoading();
            }
        }
    }
}
