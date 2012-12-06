﻿using System;
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
    public class CreatorInfo
    {
        [DataMember]
        public string GroupTitle { get; set; }
        [DataMember]
        public string GroupImageUrl { get; set; }

        public string outPut()
        {
            return "GroupTitle" + ":" + this.GroupTitle + "   " + "GroupImageUrl" + ":" + this.GroupImageUrl + "   "; 
        }
    }
}