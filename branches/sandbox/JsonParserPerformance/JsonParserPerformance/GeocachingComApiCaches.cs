using System.Collections.Generic;
using System.Runtime.Serialization;

namespace JsonParserPerformance
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
}
