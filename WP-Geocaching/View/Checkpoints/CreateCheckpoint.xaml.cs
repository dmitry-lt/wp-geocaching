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
    public partial class CreateCheckpoint : PhoneApplicationPage
    {
        public CreateCheckpoint()
        {
            InitializeComponent();
            this.DataContext = new CreateCheckpointViewModel();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            CacheDataBase db = new CacheDataBase();
            double latitude = Convert.ToDouble(this.Latitude.Text);
            double longitude = Convert.ToDouble(this.Longitude.Text);
            if (IsCorrectData(latitude, longitude))
            {
                db.AddCheckpoint(MapManager.Instance.CacheId, this.Name.Text, latitude, longitude);
                this.NavigationService.GoBack();
            }
        }
        private bool IsCorrectData(double latitude, double longitude)
        {
            if ((latitude <= 90) && (latitude >= -90) && (longitude <= 180) && (longitude >= -180))
            {
                return true;
            }
            return false;
        }
    }
}
