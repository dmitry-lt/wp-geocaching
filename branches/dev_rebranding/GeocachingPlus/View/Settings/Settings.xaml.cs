using Microsoft.Phone.Controls;
using GeocachingPlus.ViewModel;

namespace GeocachingPlus.View.Settings
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