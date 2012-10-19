using System.Data.Common;
using System.Data.Linq;
using System.Linq;
using Microsoft.Phone.Data.Linq;

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

        public Table<TEntity> VerifyTable<TEntity>() where TEntity : class
        {
            var table = GetTable<TEntity>();
            try
            {
                // can call any function against the table to verify it exists
                table.Any();
            }
            catch (DbException exception)
            {
                try
                {
                    var databaseSchemaUpdater = this.CreateDatabaseSchemaUpdater();
                    databaseSchemaUpdater.AddTable<TEntity>();
                    databaseSchemaUpdater.Execute();
                }
                catch
                {
                }
            }
            return table;
        }
    }
}
