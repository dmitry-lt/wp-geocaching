using Microsoft.Phone.Controls;
using System.Device.Location;
using System.Windows.Navigation;
using System;
using WP_Geocaching.Model.Api;
using WP_Geocaching.ViewModel;
using WP_Geocaching.Model;
using WP_Geocaching.Model.DataBase;

namespace WP_Geocaching.View.Compass
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

            var currentCacheId = NavigationContext.QueryString[NavigationManager.Params.Id.ToString()];
            var currentCacheProvider = (CacheProvider)Enum.Parse(typeof(CacheProvider), NavigationContext.QueryString[NavigationManager.Params.CacheProvider.ToString()], false);
            var currentCache = ApiManager.Instance.GetCache(currentCacheId, currentCacheProvider);

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
                    var cache = db.GetCache(currentCacheId, currentCacheProvider);
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