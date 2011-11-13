using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml;
using System.Text;
using System.IO;
using System.Xml.Linq;
using WP_Geocaching.Model;
using System.ComponentModel;

namespace WP_Geocaching.ViewModel
{
    public class BingMapViewModel
    {
        private IApiManager apiManager;
        public BingMapViewModel(IApiManager apiManager)
        {
            this.apiManager = apiManager;
        }       
    }
}
