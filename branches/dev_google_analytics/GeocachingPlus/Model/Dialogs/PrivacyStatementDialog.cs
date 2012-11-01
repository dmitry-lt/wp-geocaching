using System.Windows;
using GeocachingPlus.Resources.Localization;

namespace GeocachingPlus.Model.Dialogs
{
    public static class PrivacyStatementDialog
    {
        public static void Show()
        {
            MessageBox.Show(AppResources.PrivacyStatement, AppResources.PrivacyStatementTitle, MessageBoxButton.OK);
        }
    }
}
