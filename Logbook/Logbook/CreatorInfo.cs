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

namespace Logbook
{
    public class CreatorInfo
    {
        public string GroupTitle {get {return groupTitle;}}
        public string GroupImageUrl { get { return groupImageUrl;}}

        private string groupTitle;
        private string groupImageUrl;

    }
}
