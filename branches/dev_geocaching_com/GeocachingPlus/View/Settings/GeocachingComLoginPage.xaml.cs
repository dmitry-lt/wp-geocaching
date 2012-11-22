using Microsoft.Phone.Controls;
using GeocachingPlus.ViewModel;

namespace GeocachingPlus.View.Settings
{
    public partial class GeocachingComLoginPage : PhoneApplicationPage
    {
        public GeocachingComLoginPage()
        {
            InitializeComponent();
            var vm = new GeocachingComLoginPageViewModel(Dispatcher);
            DataContext = vm;
            vm.LoginSucceeded += (s, e) => NavigationService.GoBack();
        }
    }
}