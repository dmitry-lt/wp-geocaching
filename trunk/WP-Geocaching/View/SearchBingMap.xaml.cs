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
using Microsoft.Phone.Shell;
using WP_Geocaching.Resources.Localization;

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
            SetCheckpointsButton();
            SetSetAllButton();
        }

        private void Pushpin_Tap(object sender, GestureEventArgs e)
        {
            Pushpin pin = sender as Pushpin;
            ICommand showDetails = ((ICommand)pin.Tag);
            if (showDetails != null)
            {
                showDetails.Execute(null);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New)
            {
                int cacheId = Convert.ToInt32(NavigationContext.QueryString["ID"]);
                searchBingMapViewModel.SoughtCache = GeocahingSuApiManager.Instance.GetCachebyId(cacheId);
            }
            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.Back)
            {
                searchBingMapViewModel.UpdateMapChildrens();
            }
        }

        private void SetSetAllButton()
        {
            ApplicationBarIconButton button = new ApplicationBarIconButton();
            button.IconUri = new Uri("Resources/Images/appbar.refresh.rest.png", UriKind.Relative);
            button.Text = AppResources.ShowAllButton;
            button.Click += ShowAll_Click;
            ApplicationBar.Buttons.Add(button);
        }

        private void SetCheckpointsButton()
        {
            ApplicationBarIconButton button = new ApplicationBarIconButton();
            button.IconUri = new Uri("Resources/Images/appbar.checkpoints.png", UriKind.Relative);
            button.Text = AppResources.CheckpointsButton;
            button.Click += Checkpoints_Click;
            ApplicationBar.Buttons.Add(button);
        }

        private void ShowAll_Click(object sender, EventArgs e)
        {
            searchBingMapViewModel.ShowAll();
        }

        private void Checkpoints_Click(object sender, EventArgs e)
        {
            NavigationManager.Instance.NavigateToCheckpoints();
        }
    }
}