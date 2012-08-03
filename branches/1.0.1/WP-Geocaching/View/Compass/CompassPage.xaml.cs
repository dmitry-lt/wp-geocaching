using Microsoft.Phone.Controls;
using System.Device.Location;
using System.Windows.Navigation;
using System;
using WP_Geocaching.ViewModel;
using WP_Geocaching.Model;
using WP_Geocaching.Model.DataBase;

namespace WP_Geocaching.View.Compass
{
    public partial class CompassPage : PhoneApplicationPage
    {
        private CompassPageViewModal compassPageViewModal;

        public CompassPage()
        {
            InitializeComponent();
            compassPageViewModal = new CompassPageViewModal();
            DataContext = compassPageViewModal;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SmoothCompassManager.Instance.AddSubscriber(compassPageViewModal);

            var currentId = Convert.ToInt32(NavigationContext.QueryString[NavigationManager.Params.Id.ToString()]);
            var checkpointId = Convert.ToInt32(NavigationContext.QueryString[NavigationManager.Params.CheckpointId.ToString()]);

            if (e.NavigationMode == NavigationMode.New)
            {
                var db = new CacheDataBase();

                if (checkpointId != -1)
                {
                    var checkpoint = db.GetCheckpointByCacheIdAndCheckpointId(currentId, checkpointId);
                    compassPageViewModal.SoughtPoint = new GeoCoordinate(checkpoint.Latitude, checkpoint.Longitude);
                }
                else
                {
                    var cache = db.GetCache(currentId);
                    compassPageViewModal.SoughtPoint = new GeoCoordinate(cache.Latitude, cache.Longitude);
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SmoothCompassManager.Instance.RemoveSubscriber(compassPageViewModal);
        }
    }
}