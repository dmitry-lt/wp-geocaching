using System.Collections.Generic;
using System.Linq;
using WP_Geocaching.Model.DataBase;
using WP_Geocaching.Model;

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
                    NotifyPropertyChanged("DataSource");
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
                    ShowDetails(value.Id.ToString());
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

        private void ShowDetails(string cacheId)
        {
            NavigationManager.Instance.NavigateToInfoPivot(cacheId, true);
        }
    }
}

