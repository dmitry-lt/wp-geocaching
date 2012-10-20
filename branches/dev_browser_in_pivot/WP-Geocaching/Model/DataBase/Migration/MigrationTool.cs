using System.Linq;
using Microsoft.Phone.Data.Linq;
using WP_Geocaching.Model.Api;
using WP_Geocaching.Model.DataBase.Migration.SchemaVersion1;
using WP_Geocaching.Model.DataBase.Migration.SchemaVersion2;

namespace WP_Geocaching.Model.DataBase.Migration
{
    public class MigrationTool
    {
        private const string ConnectionString = "Data Source=isostore:/DataBase.sdf";

        public static void MigrateToVersion2()
        {
            using (var db = new CacheDataContext(ConnectionString))
            {
                var dbUpdater = db.CreateDatabaseSchemaUpdater();

                if (dbUpdater.DatabaseSchemaVersion < 2)
                {

                    // Verify tables to migrate exist
                    db.VerifyTable<MigrationDbCacheItem1>();
                    db.VerifyTable<MigrationDbCacheItem2>();
                    db.VerifyTable<MigrationDbCheckpointItem1>();
                    db.VerifyTable<MigrationDbCheckpointItem2>();

                    // Migrate caches
                    foreach (var c1 in from c1 in db.GetTable<MigrationDbCacheItem1>() select c1)
                    {
                        var c2 =
                            new MigrationDbCacheItem2()
                            {
                                Id = c1.Id + "",
                                CacheProvider = CacheProvider.GeocachingSu,
                                Name = c1.Name,
                                Latitude = c1.Latitude,
                                Longitude = c1.Longitude,
                                Type = c1.Type,
                                Subtype = c1.Subtype,
                                UpdateTime = c1.UpdateTime,
                                Description = c1.Details,
                                Logbook = c1.Notebook,
                            };

                        db.GetTable<MigrationDbCacheItem2>().InsertOnSubmit(c2);
                    }

                    // Migrate checkpoints
                    foreach (var c1 in from c1 in db.GetTable<MigrationDbCheckpointItem1>() select c1)
                    {
                        var c2 =
                            new MigrationDbCheckpointItem2()
                            {
                                Id = c1.Id,
                                CacheId = c1.CacheId + "",
                                CacheProvider = CacheProvider.GeocachingSu,
                                Name = c1.Name,
                                Latitude = c1.Latitude,
                                Longitude = c1.Longitude,
                                Type = c1.Type,
                                Subtype = c1.Subtype,
                            };

                        db.GetTable<MigrationDbCheckpointItem2>().InsertOnSubmit(c2);
                    }

                    db.SubmitChanges();

                    // Add the new database version.
                    dbUpdater.DatabaseSchemaVersion = 2;

                    // Perform the database update in a single transaction.
                    dbUpdater.Execute();
                }
            }
        }
    }
}
