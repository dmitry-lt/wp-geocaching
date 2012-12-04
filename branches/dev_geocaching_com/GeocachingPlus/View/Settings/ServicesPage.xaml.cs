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

        private void OK_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}