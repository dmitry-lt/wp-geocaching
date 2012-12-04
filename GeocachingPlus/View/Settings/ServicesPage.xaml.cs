using System.Windows;
using Microsoft.Phone.Controls;

namespace GeocachingPlus.View.Settings
{
    public partial class ServicesPage : PhoneApplicationPage
    {
        public ServicesPage()
        {
            InitializeComponent();
        }

        private void Ok_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}