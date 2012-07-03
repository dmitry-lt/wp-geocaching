using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
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

        private void Map_ViewChangeEnd(object sender, Microsoft.Phone.Controls.Maps.MapEventArgs e)
        {
            var map = sender as Microsoft.Phone.Controls.Maps.Map;
            this.bingMapViewModel.BoundingRectangle = map.BoundingRectangle;
        }

        private void Pushpin_Tap(object sender, GestureEventArgs e)
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