using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Phone.Controls;
using GeocachingPlus.Model.Api;
using GeocachingPlus.Model.Navigation;
using GeocachingPlus.ViewModel;
using GeocachingPlus.Model;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Shell;
using GeocachingPlus.Resources.Localization;
using GeocachingPlus.View.Converters;

namespace GeocachingPlus.View
{
    public partial class BingMap : PhoneApplicationPage
    {
        BingMapViewModel bingMapViewModel;

        public BingMap()
        {
            InitializeComponent();
            bingMapViewModel = new BingMapViewModel(ApiManager.Instance);
            DataContext = bingMapViewModel;
            var b = new Binding("MapMode");
            SetBinding(MapModeProperty, b);
            SetMyLocationButton();
            SetSettingsMenuItem();
        }

        private void MapViewChangeEnd(object sender, MapEventArgs e)
        {
            var map = sender as Map;
            bingMapViewModel.BoundingRectangle = map.BoundingRectangle;
        }

        private void PushpinTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var pin = sender as Pushpin;
            var showDetails = ((ICommand)pin.Tag);
            if (showDetails != null)
            {
                showDetails.Execute(null);
            }
        }

        private void SetMyLocationButton()
        {
            var button = new ApplicationBarIconButton
                             {
                                 IconUri = new Uri("Resources/Images/my.location.png", UriKind.Relative),
                                 Text = AppResources.MyLocationButton
                             };
            button.Click += MyLocationClick;
            ApplicationBar.Buttons.Add(button);
        }

        private void SetSettingsMenuItem()
        {
            var menuItem = new ApplicationBarMenuItem
            {
                Text = AppResources.SettingsMenuItem
            };
            menuItem.Click += SettingsClick;
            ApplicationBar.MenuItems.Add(menuItem);
        }

        private void MyLocationClick(object sender, EventArgs e)
        {
            bingMapViewModel.SetMapCenterOnCurrentLocationOrShowMessage(Dispatcher);
        }

        private void SettingsClick(object sender, EventArgs e)
        {
            NavigationManager.Instance.NavigateToSettings();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            bingMapViewModel.UpdateMapChildrens();
            base.OnNavigatedTo(e);
        }

        //DependencyProperty. No need for corresponding CLR-property.
        public static readonly DependencyProperty MapModeProperty =
            DependencyProperty.Register("MapMode", typeof(MapMode), typeof(BingMap),
            new PropertyMetadata(OnMapModeChanged));

        //Callback
        private static void OnMapModeChanged(DependencyObject element,
               DependencyPropertyChangedEventArgs e)
        {
            var mm = (new MapModeConverter()).Convert(e.NewValue, null, null, null);
            ((BingMap)element).Map.Mode = (Microsoft.Phone.Controls.Maps.Core.MapMode)(mm);
        }
    }
}