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
using WP_Geocaching.Model;

namespace WP_Geocaching.ViewModel
{
    public class FavoriteClass : DbCacheItem
    {
        private Enum[] iconUri;
        public Enum[] IconUri
        {
            get
            {
                return this.iconUri;
            }
            set
            {
                this.iconUri = value;
            }
        }
        public FavoriteClass(DbCacheItem item)
        {
            this.Id = item.Id;
            this.Latitude = item.Latitude;
            this.Longitude = item.Longitude;
            this.Name = item.Name;
            this.Subtype = item.Subtype;
            this.Type = item.Type;
            this.Details = item.Details;
            this.IconUri = new Enum[2]{(Cache.Types)item.Type, (Cache.Subtypes)item.Subtype};
        }
    }
}
