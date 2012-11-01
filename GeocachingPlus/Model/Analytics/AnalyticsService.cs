using System.ComponentModel.Composition.Hosting;
using System.Windows;
using Google.WebAnalytics;
using Microsoft.WebAnalytics;
using Microsoft.WebAnalytics.Behaviors;
using Microsoft.WebAnalytics.Data;

namespace GeocachingPlus.Model.Analytics
{
    public class AnalyticsService : IApplicationService
    {
        private readonly IApplicationService _innerService;
        private readonly GoogleAnalytics _googleAnalytics;

        private const string DebugTrackingId = "UA-36031366-2";
        private const string ReleaseTrackingId = "UA-36031366-1";

        public string GoogleAnalyticsTrackingId
        {

            get
            {
#if DEBUG
                return DebugTrackingId;
#endif
                return ReleaseTrackingId;
            }
        }

        public AnalyticsService()
        {
            _googleAnalytics = new GoogleAnalytics();
            _googleAnalytics.CustomVariables.Add(new PropertyValue { PropertyName = "Device ID", Value = AnalyticsProperties.DeviceId });
            _googleAnalytics.CustomVariables.Add(new PropertyValue { PropertyName = "Application Version", Value = AnalyticsProperties.ApplicationVersion });
            _googleAnalytics.CustomVariables.Add(new PropertyValue { PropertyName = "Device OS", Value = AnalyticsProperties.OsVersion });
            _googleAnalytics.CustomVariables.Add(new PropertyValue { PropertyName = "Device", Value = AnalyticsProperties.Device });

            _googleAnalytics.WebPropertyId = GoogleAnalyticsTrackingId;

            _innerService = new WebAnalyticsService
            {
                IsPageTrackingEnabled = false,
                Services = { _googleAnalytics, }
            };
        }

        #region IApplicationService Members

        public void StartService(ApplicationServiceContext context)
        {
            CompositionHost.Initialize(
                new AssemblyCatalog(
                    Application.Current.GetType().Assembly),
                new AssemblyCatalog(typeof(AnalyticsEvent).Assembly),
                new AssemblyCatalog(typeof(TrackAction).Assembly));
            _innerService.StartService(context);
        }

        public void StopService()
        {
            _innerService.StopService();
        }

        #endregion
    }
}
