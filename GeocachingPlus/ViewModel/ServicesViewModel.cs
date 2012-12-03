using System.Windows.Input;
using GeocachingPlus.Model;
using GeocachingPlus.Model.Navigation;

namespace GeocachingPlus.ViewModel
{
    public class ServicesViewModel : BaseViewModel
    {
        private Settings settings = new Settings();

        private void ShowGeocachingComLogin(object p)
        {
            NavigationManager.Instance.NavigateToGeocachingComLogin();
        }
        
        public ICommand ShowGeocachingComLoginCommand { get { return new ButtonCommand(ShowGeocachingComLogin); } }

        private bool CanAssignCacheProviderValue(bool value)
        {
            if (value)
            {
                return true;
            }

            var count = 0;
            if (IsOpenCachingComEnabled)
            {
                count++;
            }
            if (IsGeocachingSuEnabled)
            {
                count++;
            }
            if (IsGeocachingComEnabled)
            {
                count++;
            }
            return count > 1;
        }

        public bool IsOpenCachingComEnabled
        {
            get { return settings.IsOpenCachingComEnabled; }
            set
            {
                if (CanAssignCacheProviderValue(value))
                {
                    settings.IsOpenCachingComEnabled = value;
                }
                RaisePropertyChanged(() => IsOpenCachingComEnabled);
            }
        }
        public bool IsGeocachingSuEnabled
        {
            get { return settings.IsGeocachingSuEnabled; }
            set
            {
                if (CanAssignCacheProviderValue(value))
                {
                    settings.IsGeocachingSuEnabled = value;
                }
                RaisePropertyChanged(() => IsGeocachingSuEnabled);
            }
        }

        public bool IsGeocachingComEnabled
        {
            get { return settings.IsGeocachingComEnabled; }
            set
            {
                if (CanAssignCacheProviderValue(value))
                {
                    settings.IsGeocachingComEnabled = value;
                }
                RaisePropertyChanged(() => IsGeocachingComEnabled);
            }
        }

    }
}
