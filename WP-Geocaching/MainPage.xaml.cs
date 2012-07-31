using System.Windows;
using Microsoft.Phone.Controls;
using WP_Geocaching.ViewModel.MainPageViewModel;

namespace WP_Geocaching
{
    public partial class MainPage : PhoneApplicationPage
    {
        private MainPageViewModel viewModel;

        public MainPage()
        {
            InitializeComponent();

            viewModel = new MainPageViewModel();
            DataContext = viewModel;
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            viewModel.NoSouhgtCacheMessageVisibility = Visibility.Collapsed;
            viewModel.NoFavoriteCachesMessageVisibility = Visibility.Collapsed;
        }
    }
}