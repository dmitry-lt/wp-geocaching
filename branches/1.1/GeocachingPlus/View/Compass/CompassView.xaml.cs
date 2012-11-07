using System.Windows.Controls;

namespace GeocachingPlus.View.Compass
{

    public partial class CompassView : UserControl
    {

        public CompassView()
        {
            InitializeComponent();

        }

        //        private void DrawCompass()
        //        {
        //            // Update the line objects to graphically display the headings
        //            double centerX = CompassRose.ActualWidth / 2.0;
        //            double centerY = CompassRose.ActualHeight / 2.0;
        //
        //            CacheNiddle.X2 = centerX - NeedleLength * Math.Sin(MathHelper.ToRadians((float)needleDirection));
        //            CacheNiddle.Y2 = centerY - NeedleLength * Math.Cos(MathHelper.ToRadians((float)needleDirection));
        //
        //            CompassRoseRotating.Angle = -needleDirection;
        //            FpsTextBox.Text = "" + 1000 / (DateTime.Now - time).Milliseconds;
        //            time = DateTime.Now;
        //        }
    }
}
