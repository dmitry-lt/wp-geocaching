using System;
using Microsoft.Phone.Controls;
using GeocachingPlus.ViewModel;

namespace GeocachingPlus.View.Settings
{
    public partial class GeocachingComLoginPage : PhoneApplicationPage
    {
        public GeocachingComLoginPage()
        {
            InitializeComponent();
            var vm = ((GeocachingComLoginPageViewModel)DataContext);
            vm.LoginSucceeded -= GoBack;
            vm.LoginSucceeded += GoBack;
        }

        private void GoBack(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}