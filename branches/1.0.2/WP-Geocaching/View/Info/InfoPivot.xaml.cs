﻿using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Navigation;
using System.Windows.Controls;
using WP_Geocaching.Model.Dialogs;
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
            infoPivotViewModel = new InfoPivotViewModel(UpdateFavoriteButton);
            DataContext = infoPivotViewModel;
            db = new CacheDataBase();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                var cacheId = Convert.ToInt32(NavigationContext.QueryString[NavigationManager.Params.Id.ToString()]);
                infoPivotViewModel.Cache = GeocahingSuApiManager.Instance.GetCacheById(cacheId);
                var isAppBarEnabled = Convert.ToBoolean(NavigationContext.QueryString[NavigationManager.Params.IsAppBarEnabled.ToString()]);
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
            if (new Model.Settings().IsLocationEnabled)
            {
                NavigationManager.Instance.NavigateToSearchBingMap(infoPivotViewModel.Cache.Id.ToString());
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
            infoPivotViewModel.ShowConfirmDeleteDialog(Dispatcher);
        }

        private void ImageGotFocus(object sender, RoutedEventArgs e)
        {
            Photo photo = ((Image) sender).Source;
            var index = infoPivotViewModel.Previews.IndexOf(photo);
            NavigationManager.Instance.NavigateToPhotoGallery(index);   
        }
    }
}