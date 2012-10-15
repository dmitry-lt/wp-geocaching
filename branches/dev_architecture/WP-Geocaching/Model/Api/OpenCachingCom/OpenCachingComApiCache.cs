using System.Runtime.Serialization;

namespace WP_Geocaching.Model.Api.OpenCachingCom
{
    #region Response example

    /* Response example:
     
   {
      "name":"Mayak",
      "location":{
         "lat":60.11895,
         "lon":31.0806167
      },
      "type":"Traditional Cache",
      "size":3.0,
      "description":"",
      "tags":[

      ],
      "status":"Active",
      "terrain":2.5,
      "difficulty":1.5,
      "hint":null,
      "awesomeness":3.0,
      "found":null,
      "votes":null,
      "series":null,
      "images":[

      ],
      "logs":[

      ],
      "verification":{
         "number":false,
         "chirp":null,
         "code_phrase":null,
         "qr":false
      },
      "region":{
         "state":"Leningrad",
         "country":"Russia",
         "county":null,
         "city":"Osinovets",
         "postal":null
      },
      "oxcode":"OX2DH02",
      "log_code_required":false,
      "hidden_by":{
         "name":"MERDIAN",
         "id":"3693"
      },
      "hidden":1281942000000,
      "last_updated":1291819329000,
      "last_found":1335703860000,
      "review_start":null,
      "tag_votes":[

      ]
   },
      
     */

    #endregion

    [DataContract]
    public class OpenCachingComApiCache
    {
        [DataMember]
        public string oxcode { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string type { get; set; }

    }
}
