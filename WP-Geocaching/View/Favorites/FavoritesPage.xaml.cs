using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using WP_Geocaching.ViewModel;

namespace WP_Geocaching.View.Favorites
{
    public partial class FavoritesPage : PhoneApplicationPage
    {
        public FavoritesPage()
        {
            InitializeComponent();
            this.DataContext = new FavoritesViewModel();
        }
    }
}