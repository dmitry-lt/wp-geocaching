using System;
using System.Data.Linq.Mapping;

namespace WP_Geocaching.Model.DataBase.Migration.SchemaVersion1
{
    [Table(Name = "DbCacheItem")]
    public class MigrationDbCacheItem1
    {
        [Column(IsPrimaryKey = true)]
        public int Id { get; set; }

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
        public string Details { get; set; }

        [Column(DbType = "NTEXT", UpdateCheck = UpdateCheck.WhenChanged)]
        public string Notebook { get; set; }
    }
}
