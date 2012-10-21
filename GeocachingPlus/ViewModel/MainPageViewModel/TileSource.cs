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

namespace GeocachingPlus.ViewModel.MainPageViewModel
{
    public class TileSource
    {
        public string Text
        {
            get; private set;
        }
        public Action Tag
        {
            get; private set;
        }
        public string IconUri
        {
            get; private set;
        }

        public TileSource(string text, Action action, string iconUri)
        {
            Text = text;
            Tag = action;
            IconUri = iconUri;
        }
    }
}
