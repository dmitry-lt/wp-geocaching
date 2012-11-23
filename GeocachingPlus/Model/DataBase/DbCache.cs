using System;
using System.Data.Linq.Mapping;
using GeocachingPlus.Model.Api;

namespace GeocachingPlus.Model.DataBase
{
    [Table]
    public class DbCache
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
        public string HtmlDescription { get; set; }

        [Column(DbType = "NTEXT", UpdateCheck = UpdateCheck.WhenChanged)]
        public string HtmlLogbook { get; set; }

        [Column(CanBeNull = true)]
        public bool? ReliableLocation { get; set; }

        [Column(CanBeNull = true)]
        public string Hint { get; set; }
    }
}
