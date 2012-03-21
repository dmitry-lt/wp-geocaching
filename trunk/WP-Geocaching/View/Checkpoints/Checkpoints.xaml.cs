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
using WP_Geocaching.Model;
using WP_Geocaching.ViewModel;

namespace WP_Geocaching.View.Checkpoints
{
    public partial class Checkpoints : PhoneApplicationPage
    {
        CheckpointsViewModel checkpointsViewModel;
        public Checkpoints()
        {
            InitializeComponent();
            this.checkpointsViewModel = new CheckpointsViewModel();
            this.DataContext = checkpointsViewModel;
        }

        private void AddCheckpoint_Click(object sender, EventArgs e)
        {
            NavigationManager.Instance.NavigateToCreateCheckpoint();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.Back)
            {
                this.checkpointsViewModel.UpdateDataSource();
            }
        }
    }
}