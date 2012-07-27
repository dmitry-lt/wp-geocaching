using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Phone.Controls;
using WP_Geocaching.ViewModel;
using WP_Geocaching.Model;
using Microsoft.Phone.Controls.Maps;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;
using WP_Geocaching.Resources.Localization;
using WP_Geocaching.Model.DataBase;

namespace WP_Geocaching.View
{
    public partial class SearchBingMap : PhoneApplicationPage
    {
        SearchBingMapViewModel searchBingMapViewModel;

        public SearchBingMap()
        {
            InitializeComponent();
            searchBingMapViewModel = new SearchBingMapViewModel(GeocahingSuApiManager.Instance, Map.SetView);
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

            

            if (e.NavigationMode == NavigationMode.New)
            {
                int cacheId = Convert.ToInt32(NavigationContext.QueryString["ID"]);
                searchBingMapViewModel.SoughtCache = GeocahingSuApiManager.Instance.GetCacheById(cacheId);
            }

            if (e.NavigationMode == NavigationMode.Back)
            {
                searchBingMapViewModel.UpdateMapProperties();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SmoothCompassManager.Instance.RemoveSubscriber(searchBingMapViewModel);
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
            NavigationManager.Instance.NavigateToInfoPivot(searchBingMapViewModel.SoughtCache.Id.ToString(), false);
        }

        void CompassClick(object sender, EventArgs e)
        {
            var id = -1;
            var cacheId = searchBingMapViewModel.SoughtCache.Id.ToString();
            var checkpointId = id.ToString();
            var db = new Model.DataBase.CacheDataBase();
            var checkpoints = db.GetCheckpointsByCacheId(Convert.ToInt32(cacheId));

            foreach (var c in checkpoints)
            {
                if ((Cache.Subtypes)c.Subtype != Cache.Subtypes.ActiveCheckpoint) continue;
                checkpointId = c.Id.ToString();
                break;
            }

            NavigationManager.Instance.NavigateToCompass(cacheId, checkpointId);
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
            var mm = (new Model.Converters.MapModeConverter()).Convert(e.NewValue, null, null, null);
            ((SearchBingMap)element).Map.Mode = (Microsoft.Phone.Controls.Maps.Core.MapMode)(mm);
        }
    }
}