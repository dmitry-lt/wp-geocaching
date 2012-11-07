using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WP_Geocaching.View.Compass
{
    public interface ICompassAware
    {
        void SetDirection(double direction);
    }
}
