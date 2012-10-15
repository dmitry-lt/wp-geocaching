using System.Data.Linq;

namespace WP_Geocaching.Model.DataBase
{
    public class CacheDataContext : DataContext
    {
        public CacheDataContext(string sConnectionString) : base(sConnectionString) { }

        public Table<DbCacheItem> Caches
        {
            get
            {
                return this.GetTable<DbCacheItem>();
            }
        }
        public Table<DbCheckpointsItem> Checkpoints
        {
            get
            {
                return this.GetTable<DbCheckpointsItem>();
            }
        }
    }
}
