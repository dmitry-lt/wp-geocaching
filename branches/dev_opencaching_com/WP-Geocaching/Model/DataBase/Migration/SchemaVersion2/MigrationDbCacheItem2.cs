using System;
using System.Data.Linq.Mapping;
using WP_Geocaching.Model.Api;

namespace WP_Geocaching.Model.DataBase.Migration.SchemaVersion2
{
    [Table(Name = "DbCache")]
    public class MigrationDbCacheItem2
    {
        [Column(IsPrimaryKey = true)]
        public string Id { get; set; }

        [Column(IsPrimaryKey = true)]
        public CacheProvider CacheProvider { get; set; }

        [Column()]
        public string Name { get; set; }

        [Column()]
        public double Latitude { get; set; }

        [Column()]
        public double Longitude { get; set; }

        [Column()]
        public int Type { get; set; }

        [Column()]
        public int Subtype { get; set; }

        [Column()]
        public DateTime UpdateTime { get; set; }

        [Column(DbType = "NTEXT", UpdateCheck = UpdateCheck.WhenChanged)]
        public string Description { get; set; }

        [Column(DbType = "NTEXT", UpdateCheck = UpdateCheck.WhenChanged)]
        public string Logbook { get; set; }
    }
}
