using System;
using Microsoft.Phone.Controls;
using WP_Geocaching.Model;
using WP_Geocaching.Model.Navigation;
using WP_Geocaching.ViewModel;
using Microsoft.Phone.Shell;
using WP_Geocaching.Resources.Localization;

namespace WP_Geocaching.View.Checkpoints
{
    public partial class Checkpoints : PhoneApplicationPage
    {
        CheckpointsViewModel checkpointsViewModel;

        public Checkpoints()
        {
            InitializeComponent();
            checkpointsViewModel = new CheckpointsViewModel(Dispatcher);
            DataContext = checkpointsViewModel;
            SetAddCheckpointButton();
        }

        private void SetAddCheckpointButton()
        {
            var button = new ApplicationBarIconButton
                             {
                                 IconUri = new Uri("Resources/Images/appbar.add.checkpoint.png", UriKind.Relative),
                                 Text = AppResources.AddCheckpointButton
                             };
            button.Click += (sender, e) => AddCheckpointClick(sender, e);
            ApplicationBar.Buttons.Add(button);
        }

        private void AddCheckpointClick(object sender, EventArgs e)
        {
            NavigationManager.Instance.NavigateToCreateCheckpoint();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.Back)
            {
                checkpointsViewModel.UpdateDataSource();
            }
        }
    }
}