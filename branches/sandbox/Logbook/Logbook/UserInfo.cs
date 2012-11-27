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
    public class UserInfo
    {
        public int LogID {get {return logID;}}
        public int CacheID { get { return cacheID;}}
        public string LogGuid { get { return logGuid;}}
        public string Latitude { get { return latitude;}}
        public string Longitude { get { return longitude; }}
        public string LatLonString { get { return latLonString;}}
        public string LogType { get { return logType;}}
        public string LogTypeImage { get { return logTypeImage;}}
        public string LogText { get { return logText;}}
        public DateTime Created { get { return created;}}
        public DateTime Visited { get { return visited;}}
        public string UserName { get { return userName;}}
        public int MembershipLevel { get { return membershipLevel;}}
        public int AccountID { get { return accountID;}}
        public string AccountGuid { get { return accountGuid;}}
        public string Email { get { return email;}}
        public string AvatarImage { get { return avatarImage;}}
        public int GeocacheFindCount { get { return geocacheFindCount;}}
        public int GeocacheHideCount { get { return geocacheHideCount;}}
        public int ChallengesCompleted { get { return challengesCompleted;}}
        public bool IsEncoded { get { return isEncoded;}}
        public CreatorInfo creator { get { return _creator;}}
        public string Images { get { return images;}}

        private int logID;
        private int cacheID;
        private string logGuid;
        private string latitude;
        private string longitude;
        private string latLonString;
        private string logType; //found it
        private string logTypeImage;
        private string logText;
        private DateTime created;
        private DateTime visited;
        private string userName;
        private int membershipLevel;
        private int accountID;
        private string accountGuid;
        private string email;
        private string avatarImage;
        private int geocacheFindCount;
        private int geocacheHideCount;
        private int challengesCompleted;
        private bool isEncoded;
        private CreatorInfo _creator;
        private string images;

    }
}
