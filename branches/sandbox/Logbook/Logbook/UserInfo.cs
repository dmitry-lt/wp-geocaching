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
//resharper
namespace Logbook
{
    [DataContract]
    public class UserInfo
    {
        [DataMember]
        public int LogID { get; set; }
        [DataMember]
        public int CacheID { get; set; }
        [DataMember]
        public string LogGuid { get; set; }
        [DataMember]
        public string Latitude { get; set; }
        [DataMember]
        public string Longitude { get; set; }
        [DataMember]
        public string LatLonString { get; set; }
        [DataMember]
        public string LogType { get; set; }
        [DataMember]
        public string LogTypeImage { get; set; }
        [DataMember]
        public string LogText { get; set; }
        [DataMember]
        public DateTime Created { get; set; }
        [DataMember]
        public DateTime Visited { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public int MembershipLevel { get; set; }
        [DataMember]
        public int AccountID { get; set; }
        [DataMember]
        public string AccountGuid { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string AvatarImage { get; set; }
        [DataMember]
        public int GeocacheFindCount { get; set; }
        [DataMember]
        public int GeocacheHideCount { get; set; }
        [DataMember]
        public int ChallengesCompleted { get; set; }
        [DataMember]
        public bool IsEncoded { get; set; }
        [DataMember]
        public CreatorInfo creator { get; set; }
        [DataMember]
        public string Images { get; set; }

        public string outPut()
        {
            return "LogID" + ":" + this.LogID + "   " + "CacheID" + ":" + this.CacheID + "   " + "LogGuid" + ":" + this.LogGuid + "   " + "Latitude" + ":" + this.Latitude + "   " + "Longitude" + ":" + this.Longitude + "   " + "LatLonString" + ":" + this.LatLonString + "   " + "LogType" + ":" + this.LogType + "   " + "LogTypeImage" + ":" + this.LogTypeImage + "   " + "LogText" + ":" + this.LogText + "   " + "Created" + ":" + this.Created + "   " + "Visited" + ":" + this.Visited + "   " + "UserName" + ":" + this.UserName + "   " + "MembershipLevel" + ":" + this.MembershipLevel + "   " + "AccountID" + ":" + this.AccountID + "   " + "AccountGuid" + ":" + this.AccountGuid + "   " + "Email" + ":" + this.Email + "   " + "AvatarImage" + ":" + this.AvatarImage + "   " + "GeocacheFindCount" + ":" + this.GeocacheFindCount + "   " + "GeocacheHideCount" + ":" + this.GeocacheHideCount + "   " + "ChallengesCompleted" + ":" + this.ChallengesCompleted + "   " + "IsEncoded" + ":" + this.IsEncoded + "   " + "creator" + ":" + this.creator.outPut() + "   " + "Images" + ":" + this.Images + "   ";
        }
    }
}