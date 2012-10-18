using System.Data.Linq.Mapping;

namespace WP_Geocaching.Model.DataBase.Migration.SchemaVersion1
{
    [Table(Name = "DbCheckpointsItem")]
    public class MigrationDbCheckpointItem1
    {
        [Column(IsPrimaryKey = true)]
        public int Id { get; set; }

        [Column(IsPrimaryKey = true)]
        public int CacheId { get; set; }

        [Column()]
        public string Name { get; set; }

        [Column()]
        public int Type { get; set; }

        [Column()]
        public int Subtype { get; set; }

        [Column()]
        public double Latitude { get; set; }

        [Column()]
        public double Longitude { get; set; }
    }
}
