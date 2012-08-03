using Microsoft.Phone.Controls;
using WP_Geocaching.ViewModel;

namespace WP_Geocaching.View.Settings
{
    public partial class Settings : PhoneApplicationPage
    {
        public Settings()
        {
            InitializeComponent();
            DataContext = new SettingsViewModel();
        }
    }
}