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
using WP_Geocaching.Model;

namespace WP_Geocaching.View
{
    public partial class Notebook : PhoneApplicationPage
    {
        public Notebook()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            int cacheId = Convert.ToInt32(NavigationContext.QueryString["ID"]);
            GeocahingSuApiManager.Instance.GetCacheNotebook(ProcessNotebook, cacheId);            
        }

        private void ProcessNotebook(string notebook)
        {
            Browser.NavigateToString(notebook);
        }
    }
}