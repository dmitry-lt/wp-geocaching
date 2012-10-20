using System.Collections.Generic;
using System.Linq;
using WP_Geocaching.Model.Api;
using WP_Geocaching.Model.DataBase;
using WP_Geocaching.Model;
using WP_Geocaching.Model.Navigation;

namespace WP_Geocaching.ViewModel
{
    public class FavoritesViewModel : BaseViewModel
    {
        private ListCacheItem selectedCache;
        private List<ListCacheItem> dataSource;

        public List<ListCacheItem> DataSource
        {
            get
            {
                return dataSource;
            }
            private set
            {
                bool changed = dataSource != value;
                if (changed)
                {
                    dataSource = value;
                    RaisePropertyChanged(() => DataSource);
                }
            }
        }
        public ListCacheItem SelectedCache
        {
            get { return selectedCache; }
            set
            {
                selectedCache = value;
                if (value != null)
                {
                    ShowDetails(value.Cache);
                }
            }
        }

        public FavoritesViewModel()
        {
            UpdateDataSource();
        }

        public void UpdateDataSource()
        {
            DataSource = GetDataSource();
        }

        private List<ListCacheItem> GetDataSource()
        {
            var db = new CacheDataBase();
            var dbCacheList = db.GetCacheList();
            var dataSource = dbCacheList.Select(c => new ListCacheItem(c)).ToList();
            dataSource.Sort((a, b) => b.UpdateTime.CompareTo(a.UpdateTime));
            return dataSource;
        }

        private void ShowDetails(Cache cache)
        {
            NavigationManager.Instance.NavigateToInfoPivot(cache, true);
        }
    }
}

