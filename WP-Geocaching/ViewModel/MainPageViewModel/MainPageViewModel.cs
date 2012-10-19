using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using WP_Geocaching.Model;
using WP_Geocaching.Model.DataBase;
using WP_Geocaching.Model.Dialogs;
using WP_Geocaching.Model.Navigation;
using WP_Geocaching.Resources.Localization;

namespace WP_Geocaching.ViewModel.MainPageViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        private const string ChooseCacheUri = "Resources/Images/choose_cache_menu_icon.png";
        private const string SearchCacheUri = "Resources/Images/search_map_menu_icon.png";
        private const string FavoritesUri = "Resources/Images/favorites_menu_icon.png";
        private const string SettingsUri = "Resources/Images/settings_menu_icon.png";

        private Visibility noFavoriteCachesMessageVisibility = Visibility.Collapsed;
        private Visibility noSouhgtCacheMessageVisibility = Visibility.Collapsed;

        private TileSource selectedTile;

        public ObservableCollection<TileSource> Source { get; private set; }

        public TileSource SelectedTile
        {
            get
            {
                return selectedTile;
            }
            set
            {
                selectedTile = null;
                if (value != null && value.Tag != null)
                {
                    value.Tag();
                }
                NotifyPropertyChanged("SelectedTile");
            }
        }

        public Visibility NoFavoriteCachesMessageVisibility
        {
            get
            {
                return noFavoriteCachesMessageVisibility;
            }
            set
            {
                noFavoriteCachesMessageVisibility = value;
                NotifyPropertyChanged("NoFavoriteCachesMessageVisibility");
            }
        }

        public Visibility NoSouhgtCacheMessageVisibility
        {
            get
            {
                return noSouhgtCacheMessageVisibility;
            }
            set
            {
                noSouhgtCacheMessageVisibility = value;
                NotifyPropertyChanged("NoSouhgtCacheMessageVisibility");
            }
        }

        public MainPageViewModel()
        {
            var settings = new Settings();
            if (settings.IsFirstLaunching)
            {
                PrivacyStatementDialog.Show();
                settings.IsFirstLaunching = false;
            }

            Source = new ObservableCollection<TileSource>
                         {
                             GetChooseCacheTile(),
                             GetSearchCacheTile(),
                             GetFavoritesTile(),
                             GetSettingsTile()
                         };
        }

        private TileSource GetChooseCacheTile()
        {
            return new TileSource(AppResources.ChooseCache, ChooseCache, ChooseCacheUri);
        }

        private TileSource GetSearchCacheTile()
        {
            return new TileSource(AppResources.SearchCache, SearchCache, SearchCacheUri);
        }

        private TileSource GetFavoritesTile()
        {
            return new TileSource(AppResources.Favorites, Favorites, FavoritesUri);
        }

        private TileSource GetSettingsTile()
        {
            return new TileSource(AppResources.Settings, Settings, SettingsUri);
        }

        private void ChooseCache()
        {
            NavigationManager.Instance.NavigateToBingMap();
        }

        private void Settings()
        {
            NavigationManager.Instance.NavigateToSettings();
        }

        private void Favorites()
        {
            var db = new CacheDataBase();
            var dbCacheList = db.GetCacheList();

            if (dbCacheList.Count > 0)
            {
                NavigationManager.Instance.NavigateToFavorites();
            }
            else
            {
                NoFavoriteCachesMessageVisibility = Visibility.Visible;
                var timer = new DispatcherTimer
                                {
                                    Interval = TimeSpan.FromSeconds(3)
                                };
                timer.Tick += FavoritesMessageTimerTick;
                timer.Start();
            }
        }

        private void SearchCache()
        {
            var settings = new Settings();
            if (String.IsNullOrEmpty(settings.LastSoughtCacheId))
            {
                NoSouhgtCacheMessageVisibility = Visibility.Visible;
                var timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(3)
                };
                timer.Tick += SearchMessageTimerTick;
                timer.Start();
            }
            else if (settings.IsLocationEnabled)
            {
                var db = new CacheDataBase();
                var dbCache = db.GetCache(settings.LastSoughtCacheId, settings.LastSoughtCacheProvider);
                var cache = DbConvert.ToCache(dbCache);
                NavigationManager.Instance.NavigateToSearchBingMap(cache);
            }
            else
            {
                DisabledLocationDialog.Show();
            }
        }

        private void FavoritesMessageTimerTick(object sender, EventArgs args)
        {
            var t = (DispatcherTimer)sender;
            t.Stop();
            NoFavoriteCachesMessageVisibility = Visibility.Collapsed;
        }

        private void SearchMessageTimerTick(object sender, EventArgs args)
        {
            var t = (DispatcherTimer)sender;
            t.Stop();
            NoSouhgtCacheMessageVisibility = Visibility.Collapsed;
        }
    }
}
