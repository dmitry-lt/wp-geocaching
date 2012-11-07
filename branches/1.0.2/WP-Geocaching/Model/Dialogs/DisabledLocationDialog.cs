using System.Windows;
using WP_Geocaching.Resources.Localization;

namespace WP_Geocaching.Model.Dialogs
{
    public static class DisabledLocationDialog
    {
        public static void Show()
        {
            MessageBox.Show(AppResources.DisabledLocationMessage, AppResources.DisabledLocationTitle, MessageBoxButton.OK);
        }
    }
}
