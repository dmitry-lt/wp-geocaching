using System.Windows.Media;

namespace GeocachingPlus.Model.Api.GeocachingCom
{
    public static class ColorExtensions
    {
        public static int ToArgb(this Color color)
        {
            return (color.A << 24) | (color.R << 16) | (color.G << 8) | color.B;
        }

    }
}
