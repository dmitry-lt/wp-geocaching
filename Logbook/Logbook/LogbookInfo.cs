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
    public class LogbookInfo
    {
        public string  status { get{return _status;}}
        public UserInfo[] data { get{return _data;}}
        public Page pageInfo { get { return _pageInfo;}}

        private string _status;
        private UserInfo[] _data;
        private Page _pageInfo;
    }
}