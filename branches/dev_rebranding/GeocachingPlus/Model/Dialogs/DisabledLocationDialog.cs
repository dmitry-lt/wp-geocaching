using System.Windows;
using GeocachingPlus.Resources.Localization;

namespace GeocachingPlus.Model.Dialogs
{
    public static class DisabledLocationDialog
    {
        public static void Show()
        {
            MessageBox.Show(AppResources.DisabledLocationMessage, AppResources.DisabledLocationTitle, MessageBoxButton.OK);
        }
    }
}
