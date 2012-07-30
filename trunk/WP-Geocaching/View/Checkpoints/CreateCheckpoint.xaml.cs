using System;
using Microsoft.Phone.Controls;
using WP_Geocaching.ViewModel;
using Microsoft.Phone.Shell;
using WP_Geocaching.Resources.Localization;

namespace WP_Geocaching.View.Checkpoints
{
    public partial class CreateCheckpoint : PhoneApplicationPage
    {
        CheckpointViewModel checkpointViewModel;

        public CreateCheckpoint()
        {
            InitializeComponent();
            checkpointViewModel = new CheckpointViewModel();
            DataContext = checkpointViewModel;
            CoordinateInputView.Converters.LoadedPivotItem += LoadedPivotItem;
            SetAddCheckpointButton();
        }

        void LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
            checkpointViewModel.Refresh();
        }

        private void SetAddCheckpointButton()
        {
            var button = new ApplicationBarIconButton
                             {
                                 IconUri = new Uri("Resources/Images/appbar.save.rest.png", UriKind.Relative),
                                 Text = AppResources.SaveCheckpointButton
                             };
            button.Click += SaveButtonClick;
            ApplicationBar.Buttons.Add(button);
        }

        private void SaveButtonClick(object sender, EventArgs e)
        {
            checkpointViewModel.SavePoint();
            NavigationService.GoBack();
        }
    }
}