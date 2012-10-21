using System;
using System.Windows.Data;
using Microsoft.Phone.Controls.Maps;
using GeocachingPlus.Model;

namespace GeocachingPlus.View.Converters
{
    public class MapModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            switch ((MapMode)value)
            {
                case MapMode.Road:
                    return new RoadMode();
                case MapMode.Aerial:
                    return new AerialMode(true);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
