﻿using System;
using System.Diagnostics;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Navigation;
using System.Windows.Controls;
using WP_Geocaching.ViewModel;
using WP_Geocaching.Model;
using WP_Geocaching.Model.DataBase;
using WP_Geocaching.Resources.Localization;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

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
            infoPivotViewModel = new InfoPivotViewModel();
            this.DataContext = infoPivotViewModel;
            this.db = new CacheDataBase();
            SetApplicationBarItems();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New)
            {
                int cacheId = Convert.ToInt32(NavigationContext.QueryString["ID"]);
                infoPivotViewModel.Cache = GeocahingSuApiManager.Instance.GetCacheById(cacheId);
                UpdateFavoriteButton();
            }
            else
            {
                UpdateFavoriteButton();
            }
        }

        private void Info_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Contains(NotebookPivotItem) && (NotebookBrowser.SaveToString() == ""))
            {
                infoPivotViewModel.LoadNotebookPivotItem(NotebookBrowser);
            }

            if (e.AddedItems.Contains(DetailsPivotItem) && (NotebookBrowser.SaveToString() == ""))
            {
                infoPivotViewModel.LoadDetailsPivotItem(DetailsBrowser);
            }

            if (e.AddedItems.Contains(PhotosPivotItem))
            {
                //if (PhotoGrid.Children.Count == 0)
                //{
                    GeocahingSuApiManager.Instance.DownloadPhotos(infoPivotViewModel.Cache.Id, infoPivotViewModel.ProcessUriList);
                //}
            }
        }

        private void ProcessUriList(List<string> uriList)
        {
            StackPanel photoPanel = new StackPanel();
            photoPanel.Orientation = System.Windows.Controls.Orientation.Vertical;
            PhotoGrid.Children.Add(photoPanel);
            for (int i = 0; i <= uriList.Count / 3; i++)
            {
                StackPanel panel = new StackPanel();
                panel.Orientation = System.Windows.Controls.Orientation.Horizontal;
                photoPanel.Children.Add(panel);
                for (int j = i * 3; (j < (i + 1) * 3) & (j < uriList.Count) ; j++)
                {
                    GeocahingSuApiManager.Instance.LoadPhoto(uriList[j], panel.Children.Add);
                }
            }
        }

        private void SearchCacheButton_Click(object sender, EventArgs e)
        {
            db.AddCache(infoPivotViewModel.Cache, infoPivotViewModel.Details, infoPivotViewModel.Notebook);
            NavigationManager.Instance.NavigateToSearchBingMap(infoPivotViewModel.Cache.Id.ToString());
        }

        private void SetApplicationBarItems()
        {
            SetSearchButton();
            SetFavoriteButton();
        }

        private void SetSearchButton()
        {
            ApplicationBarIconButton searchButton = new ApplicationBarIconButton();
            searchButton.IconUri = new Uri("Resources/Images/appbar.feature.search.rest.png", UriKind.Relative);
            searchButton.Text = AppResources.SearchButton;
            searchButton.Click += SearchCacheButton_Click;
            ApplicationBar.Buttons.Add(searchButton);
        }

        private void SetFavoriteButton()
        {
            if (favoriteButtonIndex < 0)
            {
                ApplicationBarIconButton favoriteButton = new ApplicationBarIconButton();
                favoriteButton.IconUri = new Uri("Resources/Images/appbar.favs.addto.rest.png", UriKind.Relative);
                favoriteButton.Text = AppResources.AddFavoritesButton;
                ApplicationBar.Buttons.Add(favoriteButton);
                favoriteButtonIndex = ApplicationBar.Buttons.IndexOf(favoriteButton);
            }
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
            db.AddCache(infoPivotViewModel.Cache, infoPivotViewModel.Details, infoPivotViewModel.Notebook);
            GetDeleteButton();
        }

        private void DeleteButtonClick(object sender, EventArgs e)
        {
            db.DeleteCache(infoPivotViewModel.Cache.Id);
            GetAddButton();
        }
    }
}