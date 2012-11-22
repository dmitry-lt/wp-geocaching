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
            Subscribe();
        }

        private void Subscribe()
        {
            var vm = ((GeocachingComLoginPageViewModel)DataContext);
            vm.LoginSucceeded += GoBack;
        }

        private void Unsubscribe()
        {
            var vm = ((GeocachingComLoginPageViewModel)DataContext);
            vm.LoginSucceeded -= GoBack;
        }

        private void GoBack(object sender, EventArgs e)
        {
            Unsubscribe();
            NavigationService.GoBack();
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            Unsubscribe();
        }

    }
}