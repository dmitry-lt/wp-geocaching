using Microsoft.Phone.Controls;
using GeocachingPlus.ViewModel;
using System.Windows.Navigation;

namespace GeocachingPlus.View.Favorites
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
            if (e.NavigationMode == NavigationMode.Back)
            {
                favoritesViewModel.UpdateDataSource();
            }
        }
    }
}