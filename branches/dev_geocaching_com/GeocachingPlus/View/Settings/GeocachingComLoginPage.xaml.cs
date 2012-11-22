using Microsoft.Phone.Controls;
using GeocachingPlus.ViewModel;

namespace GeocachingPlus.View.Settings
{
    public partial class GeocachingComLoginPage : PhoneApplicationPage
    {
        public GeocachingComLoginPage()
        {
            InitializeComponent();
            ((GeocachingComLoginPageViewModel)DataContext).LoginSucceeded += (s, e) => NavigationService.GoBack();
        }
    }
}