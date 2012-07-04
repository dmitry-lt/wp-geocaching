using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;
using WP_Geocaching.Model;
using WP_Geocaching.Model.DataBase;

namespace WP_Geocaching.View
{
    public partial class Notebook : PhoneApplicationPage
    {
        private int cacheId;

        public Notebook()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            cacheId = Convert.ToInt32(NavigationContext.QueryString["ID"]);

            CacheDataBase db = new CacheDataBase();
            DbCacheItem item = db.GetCache(cacheId);
            if ((item != null) && (item.Notebook != null))
            {
                Browser.NavigateToString(item.Notebook);
            }
            else
            {
                GeocahingSuApiManager.Instance.DownloadAndProcessNotebook(ProcessNotebook, cacheId);
            }
        }

        private void ProcessNotebook(string notebook)
        {
            CacheDataBase db = new CacheDataBase();
            if (db.GetCache(cacheId) != null)
            {
                db.UpdateCacheNotebook(notebook, cacheId);
            }
            Browser.NavigateToString(notebook);
        }
    }
}