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
            vm.LoginSuccess += (s, e) => NavigationService.GoBack();
        }
    }
}