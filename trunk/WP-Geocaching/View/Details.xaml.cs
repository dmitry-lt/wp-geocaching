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
        ApplicationBarIconButton favoriteButton;

        public Details()
        {
            InitializeComponent();
            this.detailsViewModel = new DetailsViewModel();
            this.DataContext = detailsViewModel;
            context = null;
            this.db = new CacheDataBase();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            int cacheId = Convert.ToInt32(NavigationContext.QueryString["ID"]);

            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New)
            {
                context = null;
                detailsViewModel.Cache = GeocahingSuApiManager.Instance.GetCachebyId(cacheId);
                SetFavoriteButton();
            }
            else
            {
                this.ApplicationBar.Buttons.Remove(favoriteButton);
                SetFavoriteButton();
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

        private void SetFavoriteButton()
        {
            if (db.GetCache(detailsViewModel.Cache.Id) == null)
            {
                favoriteButton = GetAddButton();
                this.ApplicationBar.Buttons.Add(favoriteButton);
            }
            else
            {
                favoriteButton = GetDelButton();
                this.ApplicationBar.Buttons.Add(favoriteButton);
            }
        }

        private ApplicationBarIconButton GetAddButton()
        {
            ApplicationBarIconButton addButton = new ApplicationBarIconButton();
            addButton.IconUri = new Uri("Resources/Images/appbar.favs.addto.rest.png", UriKind.Relative);
            addButton.Click += (sender, e) =>
            {
                db.AddCache(detailsViewModel.Cache, context);
                this.ApplicationBar.Buttons.Remove(sender);
                favoriteButton = GetDelButton();
                this.ApplicationBar.Buttons.Add(favoriteButton);
            };
            addButton.Text = AppResources.AddFavoritesButton;
            return addButton;
        }

        private ApplicationBarIconButton GetDelButton()
        {
            ApplicationBarIconButton delButton = new ApplicationBarIconButton();
            delButton.IconUri = new Uri("Resources/Images/appbar.favs.deletefrom.rest.png", UriKind.Relative);
            delButton.Click += (sender, e) =>
            {
                db.DeleteCache(detailsViewModel.Cache.Id);
                this.ApplicationBar.Buttons.Remove(sender);
                favoriteButton = GetAddButton();
                this.ApplicationBar.Buttons.Add(favoriteButton);
            };
            delButton.Text = AppResources.DeleteFavoritesButton;
            return delButton;
        }
    }
}
