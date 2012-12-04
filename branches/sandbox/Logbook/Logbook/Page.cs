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
using System.Runtime.Serialization;

namespace Logbook
{
    [DataContract]
    public class Page
    {
        [DataMember]
        public int idx { get; set; }
        [DataMember]
        public int size { get; set; }
        [DataMember]
        public int totalRows { get; set; }
        [DataMember]
        public int rows { get; set; }

        public string outPut()
        {
            return "idx" + ":" + this.idx + "   " + "size" + ":" + this.size + "   " + "totalRows" + ":" + this.totalRows + "   " + "rows" + ":" + this.rows + "   ";
        }
    }
}
