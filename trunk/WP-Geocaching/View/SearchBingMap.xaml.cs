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
using Microsoft.Phone.Controls.Maps;
using System.Windows.Navigation;

namespace WP_Geocaching.View
{
    public partial class SearchBingMap : PhoneApplicationPage
    {
        SearchBingMapViewModel searchBingMapViewModel;
        public SearchBingMap()
        {
            InitializeComponent();
            this.searchBingMapViewModel = new SearchBingMapViewModel(GeocahingSuApiManager.Instance, this.Map.SetView);
            this.DataContext = this.searchBingMapViewModel;
        }

        private void Pushpin_Tap(object sender, GestureEventArgs e)
        {
            Pushpin pin = sender as Pushpin;
            string id = (string)pin.Tag;
            if (!id.Equals("-1"))
            {
                NavigationManager.Instance.NavigateToDetails((string)pin.Tag);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            int cacheId = Convert.ToInt32(NavigationContext.QueryString["ID"]);
            searchBingMapViewModel.Cache = GeocahingSuApiManager.Instance.GetCachebyId(cacheId);
        }

        private void SetAll_Click(object sender, EventArgs e)
        {
            searchBingMapViewModel.SetViewAll();
        }
    }
}