using System;
using GeocachingPlus.Model;
using GeocachingPlus.Model.DataBase;
using GeocachingPlus.Model.Navigation;
using Microsoft.Phone.Controls;
using GeocachingPlus.ViewModel;
using Microsoft.Phone.Shell;
using GeocachingPlus.Resources.Localization;

namespace GeocachingPlus.View.Checkpoints
{
    public partial class Checkpoint : PhoneApplicationPage
    {
        CheckpointViewModel _checkpointViewModel;

        public Checkpoint()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey(NavigationManager.Params.CheckpointId.ToString()))
            {
                var checkpointId = Convert.ToInt32(NavigationContext.QueryString[NavigationManager.Params.CheckpointId.ToString()]);
                _checkpointViewModel = new CheckpointViewModel(checkpointId);
            }
            else
            {
                _checkpointViewModel = new CheckpointViewModel();
            }

            DataContext = _checkpointViewModel;
            CoordinateInputView.Converters.LoadedPivotItem += LoadedPivotItem;
            SetButtons();
        }

        private void LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
            _checkpointViewModel.Refresh();
        }

        private void SetButtons()
        {
            var saveButton = new ApplicationBarIconButton
                             {
                                 IconUri = new Uri("Resources/Images/appbar.save.rest.png", UriKind.Relative),
                                 Text = AppResources.SaveCheckpointButton
                             };
            saveButton.Click += SaveButtonClick;
            ApplicationBar.Buttons.Add(saveButton);
            if (!_checkpointViewModel.NewCheckpoint)
            {
                var deleteButton = new ApplicationBarIconButton
                {
                    IconUri = new Uri("Resources/Images/appbar.delete.rest.png", UriKind.Relative),
                    Text = AppResources.Delete
                };
                deleteButton.Click += DeleteButtonClick;
                ApplicationBar.Buttons.Add(deleteButton);
            }
        }

        private void SaveButtonClick(object sender, EventArgs e)
        {
            _checkpointViewModel.SavePoint();
            NavigationService.GoBack();
        }

        private void DeleteButtonClick(object sender, EventArgs e)
        {
            var db = new CacheDataBase();
            db.DeleteCheckpoint(MapManager.Instance.Cache, _checkpointViewModel.CheckpointId + "");
            NavigationService.GoBack();
        }
    }
}