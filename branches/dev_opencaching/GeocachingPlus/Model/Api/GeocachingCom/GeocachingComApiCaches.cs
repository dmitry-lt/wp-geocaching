using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GeocachingPlus.Model.Api.GeocachingCom
{
    [DataContract]
    public class GeocachingComApiCaches
    {
        [DataMember]
        public string[] grid { get; set; }

        [DataMember]
        public string[] keys { get; set; }

        [DataMember]
        public Dictionary<string, GeocachingComApiCache[]> data { get; set; }
    }

    [DataContract]
    public class GeocachingComApiCache
    {
        [DataMember]
        public string i { get; set; }

        [DataMember]
        public string n { get; set; }
    }


    [DataContract]
    public class GeocachingComLogbook
    {
        [DataMember]
        public string status { get; set; }

        [DataMember]
        public GeocachingComLogbookEntry[] data { get; set; }
    }

    [DataContract]
    public class GeocachingComLogbookEntry
    {
        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string LogText { get; set; }
    }

}
