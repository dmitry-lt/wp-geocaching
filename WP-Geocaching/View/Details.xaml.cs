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
        DetailsViewModel detailsViewModel;

        public Details()
        {
            InitializeComponent();
            this.detailsViewModel = new DetailsViewModel();
            this.DataContext = detailsViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
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
            GeocahingSuApiManager.Instance.GetCacheInfo(ProcessCacheInfo, cacheId);
        }

        private void ProcessCacheInfo(string info)
        {
            Browser.NavigateToString(info);
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            CacheDataBase db = new CacheDataBase();
            db.AddNewItem(detailsViewModel.Cache);
        }
    }
}
