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

namespace WP_Geocaching.Model.DataBase
{
    public class CacheContext : DataContext
    {
        public CacheContext(string sConnectionString)
            : base(sConnectionString)
        { }

        public Table<CacheClass> Caches
        {
            get
            {
                return this.GetTable<CacheClass>();
            }
        }
    }
}
