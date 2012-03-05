using System;
using System.Diagnostics;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;
using WP_Geocaching.ViewModel;
using WP_Geocaching.Model;
using WP_Geocaching.Model.DataBase;

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

        private void AddToFavoritesButton_Click(object sender, EventArgs e)
        {
            db.AddCache(detailsViewModel.Cache, context);
        }

        private void SearchCacheButton_Click(object sender, EventArgs e)
        {
            db.AddCache(detailsViewModel.Cache, context);
            NavigationManager.Instance.NavigateToSearchBingMap(detailsViewModel.Cache.Id.ToString());
        }
    }
}
