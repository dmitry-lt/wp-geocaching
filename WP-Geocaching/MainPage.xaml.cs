using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
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

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            var marketplaceDetailTask = 
                new MarketplaceDetailTask
                {
                    ContentIdentifier = "6ae6923a-f704-4960-8bef-881147f5a3bb",
                    ContentType = MarketplaceContentType.Applications
                };

            marketplaceDetailTask.Show();
        }
    }
}