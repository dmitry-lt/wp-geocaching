using Microsoft.Phone.Controls;
using System.Device.Location;
using System.Windows.Navigation;
using System;
using GeocachingPlus.Model.Api;
using GeocachingPlus.Model.Navigation;
using GeocachingPlus.ViewModel;
using GeocachingPlus.Model;
using GeocachingPlus.Model.DataBase;

namespace GeocachingPlus.View.Compass
{
    public partial class CompassPage : PhoneApplicationPage
    {
        private CompassPageViewModel compassPageViewModal;

        public CompassPage()
        {
            InitializeComponent();
            compassPageViewModal = new CompassPageViewModel();
            DataContext = compassPageViewModal;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SmoothCompassManager.Instance.AddSubscriber(compassPageViewModal);

            var currentCache = Repository.CurrentCache;

            var checkpointId = Convert.ToInt32(NavigationContext.QueryString[NavigationManager.Params.CheckpointId.ToString()]);

            if (e.NavigationMode == NavigationMode.New)
            {
                var db = new CacheDataBase();

                if (checkpointId != -1)
                {
                    var checkpoint = db.GetCheckpointByCacheAndCheckpointId(currentCache, checkpointId);
                    compassPageViewModal.SoughtPoint = new GeoCoordinate(checkpoint.Latitude, checkpoint.Longitude);
                }
                else
                {
                    var cache = db.GetCache(currentCache.Id, currentCache.CacheProvider);
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