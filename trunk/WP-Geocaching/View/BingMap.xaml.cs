using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using WP_Geocaching.ViewModel;
using WP_Geocaching.Model;

namespace WP_Geocaching.View
{
    public partial class BingMap : PhoneApplicationPage
    {
        public BingMap()
        {
            InitializeComponent();
            this.DataContext = new BingMapViewModel(new GeocahingSuApiManager());            
        }
    }
}