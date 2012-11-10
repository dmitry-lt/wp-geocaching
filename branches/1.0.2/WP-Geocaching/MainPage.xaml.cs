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
                    ContentIdentifier = "dbf554a7-8b87-4342-8c48-b426e6b9a8a4",
                    ContentType = MarketplaceContentType.Applications
                };

            marketplaceDetailTask.Show();
        }
    }
}