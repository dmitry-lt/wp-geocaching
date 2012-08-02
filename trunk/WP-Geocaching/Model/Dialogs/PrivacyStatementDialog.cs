using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WP_Geocaching.Resources.Localization;

namespace WP_Geocaching.Model.Dialogs
{
    public static class PrivacyStatementDialog
    {
        public static void Show()
        {
            MessageBox.Show(AppResources.PrivacyStatement, AppResources.PrivacyStatementTitle, MessageBoxButton.OK);
        }
    }
}
