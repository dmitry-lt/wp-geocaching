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
using WP_Geocaching.Model.DataBase;
using WP_Geocaching.Model;
using Microsoft.Phone.Shell;
using WP_Geocaching.Resources.Localization;

namespace WP_Geocaching
{
    public partial class CreateCheckpoint : PhoneApplicationPage
    {
        CheckpointViewModel checkpointViewModel;

        public CreateCheckpoint()
        {
            InitializeComponent();
            checkpointViewModel = new CheckpointViewModel();
            DataContext = checkpointViewModel;
            CoordinateInputView.Converters.LoadedPivotItem += new EventHandler<PivotItemEventArgs>(LoadedPivotItem);
            SetAddCheckpointButton();
        }

        void LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
            checkpointViewModel.Refresh();
        }

        private void SetAddCheckpointButton()
        {
            ApplicationBarIconButton button = new ApplicationBarIconButton();
            button.IconUri = new Uri("Resources/Images/appbar.save.rest.png", UriKind.Relative);
            button.Text = AppResources.SaveCheckpointButton;
            button.Click += SaveButton_Click;
            ApplicationBar.Buttons.Add(button);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            checkpointViewModel.SavePoint();
            this.NavigationService.GoBack();
        }
    }
}