using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;
using WP_Geocaching.Model.Utils;
using WP_Geocaching.ViewModel;
using WP_Geocaching.Model;
using WP_Geocaching.Model.DataBase;

namespace WP_Geocaching
{
    public partial class Details : PhoneApplicationPage
    {
        DetailsViewModel detailsViewModel;

        public Details()
        {
            InitializeComponent();
            this.detailsViewModel = new DetailsViewModel();
            this.DataContext = detailsViewModel;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Cache cache = new Cache() { Id = Convert.ToInt32(NavigationContext.QueryString["ID"]) };
            foreach (Cache p in GeocahingSuApiManager.Instance.CacheList)
            {
                if (p.Equals(cache))
                {
                    detailsViewModel.Cache = p;
                }
            }
//            this.Browser.Navigate(new Uri("http://pda.geocaching.su/cache.php?cid=" +
//                NavigationContext.QueryString["ID"], UriKind.Absolute));
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            WebClient webClient = new WebClient();

            webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(ClientDownloadStringCompleted);
            webClient.AllowReadStreamBuffering = true;
            webClient.OpenReadAsync(new Uri("http://pda.geocaching.su/cache.php?cid=" + detailsViewModel.Cache.Id));

            CacheDataBase db = new CacheDataBase();
            db.AddNewItem(detailsViewModel.Cache);
        }

        private void ClientDownloadStringCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            byte[] result = new byte[e.Result.Length];
            e.Result.Read(result, 0, result.Length);

            CP1251Encoding rus = new CP1251Encoding();
            char[] chars = rus.GetChars(result);
            string fixedString = ConvertExtendedASCII(chars);

            Browser.NavigateToString(fixedString);
        }

        //TODO: to Asynk task 
        public static string ConvertExtendedASCII(char[] text)
        {
            var answer = new StringBuilder();

            foreach (char c in text)
            {
                if (Convert.ToInt32(c) > 127)
                    answer.Append("&#" + Convert.ToInt32(c) + ";");
                else
                    answer.Append(c);
            }

            return answer.ToString();
        }
    }
}
