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
    }
}
