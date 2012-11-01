using System.ComponentModel.Composition.Hosting;
using System.Windows;
using Google.WebAnalytics;

namespace GeocachingPlus.Model.Analytics
{
    public class AnalyticsService : IApplicationService
    {
        #region IApplicationService Members

        public void StartService(ApplicationServiceContext context)
        {
            CompositionHost.Initialize(
                new AssemblyCatalog(Application.Current.GetType().Assembly),
                new AssemblyCatalog(typeof(Microsoft.WebAnalytics.AnalyticsEvent).Assembly),
                new AssemblyCatalog(typeof(Microsoft.WebAnalytics.Behaviors.TrackAction).Assembly),
                new AssemblyCatalog(typeof(GoogleAnalytics).Assembly));
        }

        public void StopService()
        {
        }

        #endregion
    }
}
