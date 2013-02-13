using System.Windows;
using GeocachingPlus.Model;
using GeocachingPlus.Model.Dialogs;
using GeocachingPlus.Model.Navigation;
using Microsoft.Phone.Controls;
using GeocachingPlus.ViewModel.MainPageViewModel;

namespace GeocachingPlus
{
    public partial class MainPage : PhoneApplicationPage
    {
        private readonly MainPageViewModel _viewModel;

        public MainPage()
        {
            InitializeComponent();

            _viewModel = new MainPageViewModel();
            DataContext = _viewModel;
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            _viewModel.NoSouhgtCacheMessageVisibility = Visibility.Collapsed;
            _viewModel.NoFavoriteCachesMessageVisibility = Visibility.Collapsed;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            var settings = new Settings();
            if (settings.IsFirstLaunching)
            {
                PrivacyStatementDialog.Show();
                settings.IsFirstLaunching = false;
                NavigationManager.Instance.NavigateToServices();
            }
        }

    }
}