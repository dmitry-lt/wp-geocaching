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
    public class LogbookInfo
    {
         [DataMember]
        public string status { get; set; }
        [DataMember]
        public UserInfo[] data { get; set; }
        [DataMember]
        public Page pageInfo { get; set; }

        public string outPutArray(UserInfo[] array)
        {
            string result = "";
            for (int i = 0; i < array.Length; i++)
            {
                result += array[i].outPut();
            }
            return result;
        }

        public string outPut()
        {
            return "status" + ":" + this.status + "   " + "data" + ":" + outPutArray(this.data) + "   " + "pageInfo" + ":" + this.pageInfo.outPut() + "   ";
        }
    }
}