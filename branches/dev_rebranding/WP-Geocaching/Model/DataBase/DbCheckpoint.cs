using System.Data.Linq.Mapping;
using WP_Geocaching.Model.Api;

namespace WP_Geocaching.Model.DataBase
{
    [Table]
    public class DbCheckpoint
    {
        [Column(IsPrimaryKey = true)]
        public int Id { get; set; }

        [Column(IsPrimaryKey = true)]
        public string CacheId { get; set; }

        [Column(IsPrimaryKey = true)]
        public CacheProvider CacheProvider { get; set; }

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
