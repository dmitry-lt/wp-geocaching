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
using System.Windows.Navigation;

namespace WP_Geocaching.View.Favorites
{
    public partial class FavoritesPage : PhoneApplicationPage
    {
        FavoritesViewModel favoritesViewModel;

        public FavoritesPage()
        {
            InitializeComponent();
            favoritesViewModel = new FavoritesViewModel();
            this.DataContext = favoritesViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.Back)
            {
                favoritesViewModel.UpdateDataSource();
            }
        }
    }
}