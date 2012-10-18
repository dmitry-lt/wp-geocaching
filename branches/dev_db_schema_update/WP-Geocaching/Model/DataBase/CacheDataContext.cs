using System.Data.Linq;

namespace WP_Geocaching.Model.DataBase
{
    public class CacheDataContext : DataContext
    {
        public CacheDataContext(string sConnectionString) : base(sConnectionString) { }

        public Table<DbCache> Caches
        {
            get
            {
                return this.GetTable<DbCache>();
            }
        }
        public Table<DbCheckpoint> Checkpoints
        {
            get
            {
                return this.GetTable<DbCheckpoint>();
            }
        }
    }
}
