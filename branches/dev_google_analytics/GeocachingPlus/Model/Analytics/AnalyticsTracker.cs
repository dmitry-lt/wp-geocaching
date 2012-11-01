using System;
using System.ComponentModel.Composition;
using Microsoft.WebAnalytics;

namespace GeocachingPlus.Model.Analytics
{
    public enum AnalyticsCategory
    {
        UserAction
    }

    public enum AnalyticsUserAction
    {
        // Main screen
        MainScreenChooseMapOpened,
        MainScreenFavoritesOpened,
        MainScreenSearchMapOpened,
        MainScreenSettingsOpened,
        MainScreenBackNavigated,

        // Choose map
        ChooseMapMyLocationSelected,
        ChooseMapSettingsOpened,
        ChooseMapCacheDetailsOpened,
        ChooseMapBackNavigated,

        // Favorites
        FavoritesCacheDetailsOpened,
        FavoritesBackNavigated,

        // Cache details
        CacheDetailsSearchMapOpened,
        CacheDetailsAddSelected,
        CacheDetailsRemoveSelected,
        CacheDetailsDetailsTabSelected,
        CacheDetailsPhotosTabSelected,
        CacheDetailsLogbookTabSelected,
        CacheDetailsBackNavigated,

        // TODO: all pages

        // Photo gallery

        // Search map

        // Compass

        // Checkpoints

        // Edit checkpoint

        // Settings
    }

    public class AnalyticsTracker
    {
        public AnalyticsTracker()
        {
            CompositionInitializer.SatisfyImports(this);
        }

        [Import("Log")]
        public Action<AnalyticsEvent> Log { get; set; }

        public void Track(string category, string name)
        {
            Track(category, name, null);
        }

        public void Track(string category, string name, string actionValue)
        {
            Log(new AnalyticsEvent { Category = category, Name = name, ObjectName = actionValue });
        }
    }
}
