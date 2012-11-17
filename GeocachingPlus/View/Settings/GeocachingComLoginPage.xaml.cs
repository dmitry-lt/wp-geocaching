using Microsoft.Phone.Controls;
using GeocachingPlus.ViewModel;
using System.Windows.Navigation;

namespace GeocachingPlus.View.Settings
{
    public partial class GeocachingComLoginPage : PhoneApplicationPage
    {
        FavoritesViewModel favoritesViewModel;

        public GeocachingComLoginPage()
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