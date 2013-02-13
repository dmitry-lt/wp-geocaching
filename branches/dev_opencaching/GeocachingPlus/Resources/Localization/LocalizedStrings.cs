using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace GeocachingPlus.Resources.Localization
{
    public class LocalizedStrings
    {
        public LocalizedStrings()
        {
        }

        private static AppResources localizedResources = new AppResources();
        private static GeocachingComLogin geocachingComLogin = new GeocachingComLogin();

        public AppResources LocalizedResources { get { return localizedResources; } }
        public GeocachingComLogin GeocachingComLogin { get { return geocachingComLogin; } }
    }
}
