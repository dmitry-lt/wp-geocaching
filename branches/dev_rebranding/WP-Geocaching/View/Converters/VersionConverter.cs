using System;
using System.Globalization;
using System.Windows.Data;
using System.Xml.Linq;

namespace GeocachingPlus.View.Converters
{
    public class VersionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var version = XDocument.Load("WMAppManifest.xml").Root.Element("App").Attribute("Version").Value;

            return String.Format(value.ToString(), version);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
