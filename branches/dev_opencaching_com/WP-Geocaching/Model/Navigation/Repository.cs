using System.Collections.Generic;

namespace WP_Geocaching.Model.Navigation
{
    public class Repository
    {
        public static List<Photo> CurrentPhotos { get; set; }
        public static Cache CurrentCache { get; set; }
    }
}
