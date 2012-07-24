using System;
using System.Windows.Input;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps.Core;
using WP_Geocaching.ViewModel;
using WP_Geocaching.Model;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Shell;
using WP_Geocaching.Resources.Localization;

namespace WP_Geocaching.View
{
    public partial class BingMap : PhoneApplicationPage
    {
        BingMapViewModel bingMapViewModel;

        public BingMap()
        {
            InitializeComponent();
            this.bingMapViewModel = new BingMapViewModel(GeocahingSuApiManager.Instance);
            this.DataContext = this.bingMapViewModel;
            SetMyLocationButton();
        }

        private void Map_ViewChangeEnd(object sender, MapEventArgs e)
        {
            var map = sender as Map;
            map.Mode = new AerialMode();
            this.bingMapViewModel.BoundingRectangle = map.BoundingRectangle;
        }

        private void Pushpin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Pushpin pin = sender as Pushpin;
            ICommand showDetails = ((ICommand)pin.Tag);
            if (showDetails != null)
            {
                showDetails.Execute(null);
            }
        }

        private void SetMyLocationButton()
        {
            ApplicationBarIconButton button = new ApplicationBarIconButton();
            button.IconUri = new Uri("Resources/Images/my.location.png", UriKind.Relative);
            button.Text = AppResources.MyLocationButton;
            button.Click += MyLocation_Click;
            ApplicationBar.Buttons.Add(button);
        }

        private void MyLocation_Click(object sender, EventArgs e)
        {
            bingMapViewModel.SetMapCenterOnCurrentLocationOrShowMessage(this.Dispatcher);
        }
    }
}