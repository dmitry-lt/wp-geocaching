using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Navigation;
using System.Windows.Controls;
using WP_Geocaching.Model.Api;
using WP_Geocaching.Model.Dialogs;
using WP_Geocaching.ViewModel;
using WP_Geocaching.Model;
using WP_Geocaching.Model.DataBase;
using WP_Geocaching.Resources.Localization;

namespace WP_Geocaching.View.Info
{
    public partial class InfoPivot : PhoneApplicationPage
    {
        private readonly InfoPivotViewModel _infoPivotViewModel;
        private readonly CacheDataBase _db;
        private int _favoriteButtonIndex = -1;

        public InfoPivot()
        {
            InitializeComponent();
            _infoPivotViewModel = new InfoPivotViewModel(UpdateFavoriteButton, InfoBrowser, LogbookBrowser);
            DataContext = _infoPivotViewModel;
            _db = new CacheDataBase();

            _infoPivotViewModel.HidePhotos += (s, e) => Info.Items.Remove(PhotosPivotItem);
        }

        private bool _isAppBarEnabled;
        private void ShowAppBar(object sender, EventArgs e)
        {
            if (_isAppBarEnabled)
            {
                ApplicationBar.IsVisible = true;
                if (ApplicationBar.IsVisible)
                {
                    SetApplicationBarItems();
                    UpdateFavoriteButton();
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                // show appbar after cache info has been loaded
                ApplicationBar.IsVisible = false;
                _isAppBarEnabled = Convert.ToBoolean(NavigationContext.QueryString[NavigationManager.Params.IsAppBarEnabled.ToString()]);
                _infoPivotViewModel.CacheFullyLoaded -= ShowAppBar;
                _infoPivotViewModel.CacheFullyLoaded += ShowAppBar;

                var cacheId = NavigationContext.QueryString[NavigationManager.Params.Id.ToString()];
                var cacheProvider = (CacheProvider)Enum.Parse(typeof(CacheProvider), NavigationContext.QueryString[NavigationManager.Params.CacheProvider.ToString()], false);
                _infoPivotViewModel.Cache = ApiManager.Instance.GetCache(cacheId, cacheProvider);
            }
            else
            {
                if (ApplicationBar.IsVisible)
                {
                    UpdateFavoriteButton();
                }
            }
        }

        private void SearchCacheButtonClick(object sender, EventArgs e)
        {
            _db.AddCache(_infoPivotViewModel.Cache, _infoPivotViewModel.Info, _infoPivotViewModel.Logbook);
            if (new Model.Settings().IsLocationEnabled)
            {
                NavigationManager.Instance.NavigateToSearchBingMap(_infoPivotViewModel.Cache.Id, _infoPivotViewModel.Cache.CacheProvider);
            }
            else
            {
                DisabledLocationDialog.Show();
            }
            
        }

        private void SetApplicationBarItems()
        {
            SetSearchButton();
            SetFavoriteButton();
        }

        private void SetSearchButton()
        {
            var searchButton = new ApplicationBarIconButton
                                   {
                                       IconUri =
                                           new Uri("Resources/Images/appbar.feature.search.rest.png", UriKind.Relative),
                                       Text = AppResources.SearchButton
                                   };
            searchButton.Click += SearchCacheButtonClick;
            ApplicationBar.Buttons.Add(searchButton);
        }

        private void SetFavoriteButton()
        {
            if (_favoriteButtonIndex >= 0) return;
            var favoriteButton = new ApplicationBarIconButton
                                     {
                                         IconUri =
                                             new Uri("Resources/Images/appbar.favs.addto.rest.png", UriKind.Relative),
                                         Text = AppResources.AddFavoritesButton
                                     };
            ApplicationBar.Buttons.Add(favoriteButton);
            _favoriteButtonIndex = ApplicationBar.Buttons.IndexOf(favoriteButton);
        }

        private void UpdateFavoriteButton()
        {
            if (_db.GetCache(_infoPivotViewModel.Cache.Id, _infoPivotViewModel.Cache.CacheProvider) == null)
            {
                GetAddButton();
            }
            else
            {
                GetDeleteButton();
            }
        }

        private void GetAddButton()
        {
            (ApplicationBar.Buttons[_favoriteButtonIndex] as ApplicationBarIconButton).IconUri =
                new Uri("Resources/Images/appbar.favs.addto.rest.png", UriKind.Relative);
            (ApplicationBar.Buttons[_favoriteButtonIndex] as ApplicationBarIconButton).Click += AddButtonClick;
            (ApplicationBar.Buttons[_favoriteButtonIndex] as ApplicationBarIconButton).Click -= DeleteButtonClick;
            (ApplicationBar.Buttons[_favoriteButtonIndex] as ApplicationBarIconButton).Text = AppResources.AddFavoritesButton;
        }

        private void GetDeleteButton()
        {
            (ApplicationBar.Buttons[_favoriteButtonIndex] as ApplicationBarIconButton).IconUri =
                new Uri("Resources/Images/appbar.favs.deletefrom.rest.png", UriKind.Relative);
            (ApplicationBar.Buttons[_favoriteButtonIndex] as ApplicationBarIconButton).Click += DeleteButtonClick;
            (ApplicationBar.Buttons[_favoriteButtonIndex] as ApplicationBarIconButton).Click -= AddButtonClick;
            (ApplicationBar.Buttons[_favoriteButtonIndex] as ApplicationBarIconButton).Text = AppResources.DeleteFavoritesButton;
        }

        private void AddButtonClick(object sender, EventArgs e)
        {
            _db.AddCache(_infoPivotViewModel.Cache, _infoPivotViewModel.Info, _infoPivotViewModel.Logbook);
            _infoPivotViewModel.DownloadAndSavePhotos();
            GetDeleteButton();
        }

        private void DeleteButtonClick(object sender, EventArgs e)
        {
            _infoPivotViewModel.ShowConfirmDeleteDialog(Dispatcher);
        }

        private void ImageGotFocus(object sender, RoutedEventArgs e)
        {
            var photo = new Photo(((Image)sender).Source);
            var index = _infoPivotViewModel.Previews.IndexOf(photo);
            NavigationManager.Instance.NavigateToPhotoGallery(index);   
        }
    }
}