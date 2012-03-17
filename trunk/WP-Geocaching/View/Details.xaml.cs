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
            context = null;
            int cacheId = Convert.ToInt32(NavigationContext.QueryString["ID"]);
            detailsViewModel.Cache = GeocahingSuApiManager.Instance.GetCachebyId(cacheId);
            SetFavoriteButton(); 
            DbCacheItem item = db.GetCache(cacheId);
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
                this.ApplicationBar.Buttons.Add(GetAddButton());
            }
            else
            {
                this.ApplicationBar.Buttons.Add(GetDelButton());
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
                this.ApplicationBar.Buttons.Add(GetDelButton());
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
                this.ApplicationBar.Buttons.Add(GetAddButton());
            };
            delButton.Text = AppResources.DeleteFavoritesButton;
            return delButton;
        }
    }
}
