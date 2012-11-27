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
    public class Page
    {
        public int idx {get {return _idx;}}
        public int size {get {return _size;}}
        public int totalRows {get {return _totalRows;}}
        public int rows {get {return _rows;}}

        private int _idx;
        private int _size;
        private int _totalRows;
        private int _rows;
    }
}
