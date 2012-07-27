using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Navigation;
using System.Windows.Controls;
using WP_Geocaching.ViewModel;
using WP_Geocaching.Model;
using WP_Geocaching.Model.DataBase;
using WP_Geocaching.Resources.Localization;

namespace WP_Geocaching.View.Info
{
    public partial class InfoPivot : PhoneApplicationPage
    {
        private InfoPivotViewModel infoPivotViewModel;
        private CacheDataBase db;
        private int favoriteButtonIndex = -1;

        public InfoPivot()
        {
            InitializeComponent();
            infoPivotViewModel = new InfoPivotViewModel(GetAddButton);
            DataContext = infoPivotViewModel;
            db = new CacheDataBase();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                var cacheId = Convert.ToInt32(NavigationContext.QueryString["ID"]);
                infoPivotViewModel.Cache = GeocahingSuApiManager.Instance.GetCacheById(cacheId);
                var isAppBarEnabled = Convert.ToBoolean(NavigationContext.QueryString["IsAppBarEnabled"]);
                ApplicationBar.IsVisible = isAppBarEnabled;
                if (ApplicationBar.IsVisible)
                {
                    SetApplicationBarItems();
                    UpdateFavoriteButton();
                }
            }
            else
            {
                if (ApplicationBar.IsVisible)
                {
                    UpdateFavoriteButton();
                }
            }
        }

        private void InfoSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Contains(NotebookPivotItem) && (NotebookBrowser.SaveToString() == ""))
            {
                infoPivotViewModel.LoadNotebookPivotItem(NotebookBrowser);
            }

            if (e.AddedItems.Contains(DetailsPivotItem) && (NotebookBrowser.SaveToString() == ""))
            {
                infoPivotViewModel.LoadDetailsPivotItem(InfoBrowser);
            }

            if (e.AddedItems.Contains(PhotosPivotItem))
            {
                infoPivotViewModel.LoadPreviews();
            }
        }

        private void SearchCacheButtonClick(object sender, EventArgs e)
        {
            db.AddCache(infoPivotViewModel.Cache, infoPivotViewModel.Info, infoPivotViewModel.Notebook);
            NavigationManager.Instance.NavigateToSearchBingMap(infoPivotViewModel.Cache.Id.ToString());
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
            if (favoriteButtonIndex >= 0) return;
            var favoriteButton = new ApplicationBarIconButton
                                     {
                                         IconUri =
                                             new Uri("Resources/Images/appbar.favs.addto.rest.png", UriKind.Relative),
                                         Text = AppResources.AddFavoritesButton
                                     };
            ApplicationBar.Buttons.Add(favoriteButton);
            favoriteButtonIndex = ApplicationBar.Buttons.IndexOf(favoriteButton);
        }

        private void UpdateFavoriteButton()
        {
            if (db.GetCache(infoPivotViewModel.Cache.Id) == null)
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
            (ApplicationBar.Buttons[favoriteButtonIndex] as ApplicationBarIconButton).IconUri =
                new Uri("Resources/Images/appbar.favs.addto.rest.png", UriKind.Relative);
            (ApplicationBar.Buttons[favoriteButtonIndex] as ApplicationBarIconButton).Click += AddButtonClick;
            (ApplicationBar.Buttons[favoriteButtonIndex] as ApplicationBarIconButton).Click -= DeleteButtonClick;
            (ApplicationBar.Buttons[favoriteButtonIndex] as ApplicationBarIconButton).Text = AppResources.AddFavoritesButton;
        }

        private void GetDeleteButton()
        {
            (ApplicationBar.Buttons[favoriteButtonIndex] as ApplicationBarIconButton).IconUri =
                new Uri("Resources/Images/appbar.favs.deletefrom.rest.png", UriKind.Relative);
            (ApplicationBar.Buttons[favoriteButtonIndex] as ApplicationBarIconButton).Click += DeleteButtonClick;
            (ApplicationBar.Buttons[favoriteButtonIndex] as ApplicationBarIconButton).Click -= AddButtonClick;
            (ApplicationBar.Buttons[favoriteButtonIndex] as ApplicationBarIconButton).Text = AppResources.DeleteFavoritesButton;
        }

        private void AddButtonClick(object sender, EventArgs e)
        {
            db.AddCache(infoPivotViewModel.Cache, infoPivotViewModel.Info, infoPivotViewModel.Notebook);
            infoPivotViewModel.DownloadAndSavePhotos();
            infoPivotViewModel.DownloadAndSaveNotebook();
            infoPivotViewModel.DownloadAndSaveCacheInfo();
            GetDeleteButton();
        }

        private void DeleteButtonClick(object sender, EventArgs e)
        {
            infoPivotViewModel.DeleteDialogVisibility = Visibility.Visible;
        }

        private void ImageGotFocus(object sender, RoutedEventArgs e)
        {
            var index = (int)(((Image)sender).Tag);
            NavigationManager.Instance.NavigateToPhotoGallery(index);
        }
    }
}