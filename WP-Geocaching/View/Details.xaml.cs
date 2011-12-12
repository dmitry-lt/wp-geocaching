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
using System.Windows.Navigation;

namespace WP_Geocaching
{
    public partial class Details : PhoneApplicationPage
    {
        public Details()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.Browser.Navigate(new Uri("http://pda.geocaching.su/cache.php?cid=" + 
                NavigationContext.QueryString["ID"], UriKind.Absolute));
        }
    }
}
