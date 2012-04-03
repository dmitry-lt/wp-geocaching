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
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            CacheDataBase db = new CacheDataBase();
            db.AddActiveCheckpoint(MapManager.Instance.CacheId, createCheckpointViewModel.Name, 
                createCheckpointViewModel.CurrentInputPointLocation.Latitude, 
                createCheckpointViewModel.CurrentInputPointLocation.Longitude);
            this.NavigationService.GoBack();
        }
    }
}
