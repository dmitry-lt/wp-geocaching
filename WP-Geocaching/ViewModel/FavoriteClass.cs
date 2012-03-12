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
using WP_Geocaching.Model.DataBase;

namespace WP_Geocaching.ViewModel
{
    public class FavoriteClass : DbCacheItem
    {
        public Uri IconUri { get; set; }
        public FavoriteClass(DbCacheItem item)
        {
            this.Id = item.Id;
            this.Latitude = item.Latitude;
            this.Longitude = item.Longitude;
            this.Name = item.Name;
            this.Subtype = item.Subtype;
            this.Type = item.Type;
            this.Details = item.Details;
            this.IconUri = new Uri((item.Type.ToString() + item.Subtype.ToString()), UriKind.Relative);
        }
    }
}
