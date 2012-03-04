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
            Cache cache = new Cache()
                {
                    Id = cacheId
                };

            foreach (Cache p in GeocahingSuApiManager.Instance.CacheList)
            {
                if (p.Equals(cache))
                {
                    detailsViewModel.Cache = p;
                    break;
                }
            }
            CacheClass item = db.GetItem(cacheId);
            if ((item != null) && (item.Details != null))
            {
                ProcessCacheInfo(item.Details);
            }
            else       
            {
                GeocahingSuApiManager.Instance.GetCacheInfo(ProcessCacheInfo, cacheId);
            }
        }

        private void ProcessCacheInfo(string info)
        {
            context = info;
            if (db.GetItem(detailsViewModel.Cache.Id) != null)
            {
                db.AddDetailsInItem(info, detailsViewModel.Cache.Id);
            }
            Browser.NavigateToString(info);
        }

        private void AddToFavoritesButton_Click(object sender, EventArgs e)
        {
            db.AddNewItem(detailsViewModel.Cache, context);
        }
    }
}
