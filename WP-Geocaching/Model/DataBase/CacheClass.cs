using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Collections.Generic;

namespace WP_Geocaching.Model.DataBase
{
    [Table]
    public class CacheClass
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
        public int Type;
        [Column()]
        public int Subtype;
    }
}
