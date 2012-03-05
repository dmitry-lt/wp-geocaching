using System.Windows;
using Microsoft.Phone.Controls;
using WP_Geocaching.ViewModel;

namespace WP_Geocaching.View.Compass
{
    public partial class CompassPage : PhoneApplicationPage
    {
        private CompassPageViewModal compassPageViewModal;

        public CompassPage()
        {
            InitializeComponent();
            compassPageViewModal = new CompassPageViewModal();
            DataContext = compassPageViewModal;
        }

        private void LayoutRootLoaded(object sender, RoutedEventArgs e)
        {
            compassPageViewModal.Start();
        }

        //TODO: don't called on win-button click
        private void LayoutRootUnloaded(object sender, RoutedEventArgs e)
        {
            // Stop data acquisition from the compass.
            compassPageViewModal.Stop();
        }
    }
}