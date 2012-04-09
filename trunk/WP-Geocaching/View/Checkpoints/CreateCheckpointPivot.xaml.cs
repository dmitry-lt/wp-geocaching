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
    public partial class CreateCheckpointPivot : PhoneApplicationPage
    {
        CreateCheckpointViewModel createCheckpointViewModel;

        public CreateCheckpointPivot()
        {
            InitializeComponent();
            createCheckpointViewModel = new CreateCheckpointViewModel();
            this.DataContext = createCheckpointViewModel;
            SetAddCheckpointButton();
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
            createCheckpointViewModel.SavePoint();
            this.NavigationService.GoBack();
        }
    }
}
