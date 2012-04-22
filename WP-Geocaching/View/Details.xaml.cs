using System;
using System.Diagnostics;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Navigation;
using WP_Geocaching.ViewModel;
using WP_Geocaching.Model;
using WP_Geocaching.Model.DataBase;
using WP_Geocaching.Resources.Localization;

namespace WP_Geocaching
{
    public partial class Details : PhoneApplicationPage
    {
        private DetailsViewModel detailsViewModel;
        private string context;
        private CacheDataBase db;
        private int favoriteButtonIndex = -1;

        public Details()
        {
            InitializeComponent();
            this.detailsViewModel = new DetailsViewModel();
            this.DataContext = detailsViewModel;
            context = null;
            this.db = new CacheDataBase();
            SetApplicationBarItems();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            int cacheId = Convert.ToInt32(NavigationContext.QueryString["ID"]);

            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New)
            {
                context = null;
                detailsViewModel.Cache = GeocahingSuApiManager.Instance.GetCachebyId(cacheId);
                UpdateFavoriteButton();
            }
            else
            {
                UpdateFavoriteButton();
            }

            DbCacheItem item = db.GetCache(detailsViewModel.Cache.Id);
            if ((item != null) && (item.Details != null))
            {
                Browser.NavigateToString(item.Details);
            }
            else       
            {
                GeocahingSuApiManager.Instance.GetCacheInfo(ProcessCacheInfo, cacheId);
            }
        }

        private void ProcessCacheInfo(string info)
        {
            context = info;
            if (db.GetCache(detailsViewModel.Cache.Id) != null)
            {
                db.UpdateCacheInfo(info, detailsViewModel.Cache.Id);
            }
            Browser.NavigateToString(info);
        }

        private void SearchCacheButton_Click(object sender, EventArgs e)
        {
            db.AddCache(detailsViewModel.Cache, context);
            NavigationManager.Instance.NavigateToSearchBingMap(detailsViewModel.Cache.Id.ToString());
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
            if (db.GetCache(detailsViewModel.Cache.Id) == null)
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
            db.AddCache(detailsViewModel.Cache, context);
            GetDeleteButton();
        }
        private void DeleteButtonClick(object sender, EventArgs e)
        {
            db.DeleteCache(detailsViewModel.Cache.Id);
            GetAddButton();
        }
    }
}
