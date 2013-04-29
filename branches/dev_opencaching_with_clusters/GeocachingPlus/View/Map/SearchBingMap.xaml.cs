using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Navigation;
using GeocachingPlus.Model;
using GeocachingPlus.Model.Api;
using GeocachingPlus.Model.Api.GeocachingSu;
using GeocachingPlus.Model.DataBase;
using GeocachingPlus.Model.Dialogs;
using GeocachingPlus.Model.Navigation;
using GeocachingPlus.Resources.Localization;
using GeocachingPlus.View.Converters;
using GeocachingPlus.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Shell;

namespace GeocachingPlus.View.Map
{
    public partial class SearchBingMap : PhoneApplicationPage
    {
        SearchBingMapViewModel searchBingMapViewModel;

        public SearchBingMap()
        {
            InitializeComponent();
            searchBingMapViewModel = new SearchBingMapViewModel(ApiManager.Instance, Map.SetView);
            DataContext = searchBingMapViewModel;
            var binding = new Binding("MapMode");
            SetBinding(MapModeProperty, binding);
            SetAppBarItems();
        }

        private void SetAppBarItems()
        {
            SetInfoButton();
            SetCompassButton();
            SetCheckpointsButton();
            SetMyLocationButton();
            SetShowAllMenuItem();
            SetSettingsMenuItem();
        }

        private void PushpinTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var pin = sender as Pushpin;
            var showDetails = ((ICommand)pin.Tag);
            if (showDetails != null)
            {
                showDetails.Execute(false);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SmoothCompassManager.Instance.AddSubscriber(searchBingMapViewModel);
            LocationManager.Instance.AddSubscriber(searchBingMapViewModel);

            var setting = new Model.Settings();
            if (!setting.IsLocationEnabled)
            {
                DisabledLocationDialog.Show();
            }

            if (e.NavigationMode == NavigationMode.New)
            {
                searchBingMapViewModel.SoughtCache = Repository.CurrentCache;
            }

            if (e.NavigationMode == NavigationMode.Back)
            {
                searchBingMapViewModel.UpdateMapProperties();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SmoothCompassManager.Instance.RemoveSubscriber(searchBingMapViewModel);
            LocationManager.Instance.RemoveSubscriber(searchBingMapViewModel);
            base.OnNavigatedFrom(e);
        }

        private void SetShowAllMenuItem()
        {
            var menuItem = new ApplicationBarMenuItem
                             {
                                 Text = AppResources.ShowAllMenuItem
                             };
            menuItem.Click += ShowAllClick;
            ApplicationBar.MenuItems.Add(menuItem);
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

        private void SetCheckpointsButton()
        {
            var button = new ApplicationBarIconButton
                             {
                                 IconUri = new Uri("Resources/Images/appbar.checkpoints.png", UriKind.Relative),
                                 Text = AppResources.CheckpointsButton
                             };
            button.Click += CheckpointsClick;
            ApplicationBar.Buttons.Add(button);
        }

        private void SetCompassButton()
        {
            var button = new ApplicationBarIconButton
                             {
                                 IconUri = new Uri("Resources/Images/appbar.compass.png", UriKind.Relative),
                                 Text = AppResources.CompassSearchButton
                             };
            button.Click += CompassClick;
            ApplicationBar.Buttons.Add(button);
        }

        private void SetInfoButton()
        {
            var button = new ApplicationBarIconButton
            {
                IconUri = new Uri("Resources/Images/appbar.info.png", UriKind.Relative),
                Text = AppResources.InfoButton
            };
            button.Click += ShowInfo;
            ApplicationBar.Buttons.Add(button);
        }

        void ShowInfo(object sender, EventArgs e)
        {
            NavigationManager.Instance.NavigateToInfoPivot(searchBingMapViewModel.SoughtCache, false);
        }

        void CompassClick(object sender, EventArgs e)
        {
            var id = -1;
            var cache = searchBingMapViewModel.SoughtCache;
            var cacheProvider = searchBingMapViewModel.SoughtCache.CacheProvider;
            var checkpointId = id.ToString();
            var db = new CacheDataBase();
            var checkpoints = db.GetCheckpointsByCache(cache);

            foreach (var c in checkpoints)
            {
                if ((GeocachingSuCache.Subtypes)c.Subtype != GeocachingSuCache.Subtypes.ActiveCheckpoint) continue;
                checkpointId = c.Id.ToString();
                break;
            }

            NavigationManager.Instance.NavigateToCompass(cache, checkpointId);
        }

        private void ShowAllClick(object sender, EventArgs e)
        {
            searchBingMapViewModel.ShowAll();
        }

        private void CheckpointsClick(object sender, EventArgs e)
        {
            NavigationManager.Instance.NavigateToCheckpoints();
        }

        private void MyLocationClick(object sender, EventArgs e)
        {
            searchBingMapViewModel.SetMapCenterOnCurrentLocationOrShowMessage(Dispatcher);
        }

        private void SettingsClick(object sender, EventArgs e)
        {
            NavigationManager.Instance.NavigateToSettings();
        }

        public static readonly DependencyProperty MapModeProperty =
            DependencyProperty.Register("MapMode", typeof(MapMode), typeof(SearchBingMap),
            new PropertyMetadata(OnMapModeChanged));


        private static void OnMapModeChanged(DependencyObject element,
               DependencyPropertyChangedEventArgs e)
        {
            var mm = (new MapModeConverter()).Convert(e.NewValue, null, null, null);
            ((SearchBingMap)element).Map.Mode = (Microsoft.Phone.Controls.Maps.Core.MapMode)(mm);
        }
    }
}