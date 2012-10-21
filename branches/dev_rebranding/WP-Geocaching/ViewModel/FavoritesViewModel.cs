using System.Collections.Generic;
using System.Linq;
using GeocachingPlus.Model.Api;
using GeocachingPlus.Model.DataBase;
using GeocachingPlus.Model;
using GeocachingPlus.Model.Navigation;

namespace GeocachingPlus.ViewModel
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

