using Microsoft.Phone.Controls;
using GeocachingPlus.ViewModel;

namespace GeocachingPlus.View.Settings
{
    public partial class GeocachingComLoginPage : PhoneApplicationPage
    {
        public GeocachingComLoginPage()
        {
            InitializeComponent();
            DataContext = new GeocachingComLoginPageViewModel();
        }
    }
}