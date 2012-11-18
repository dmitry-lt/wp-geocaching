using System;
using System.IO;
using System.Net;
using System.Security;

namespace GeocachingPlus.Model.Api.GeocachingCom
{
    public class ExtendedWebClient : WebClient
    {
        public CookieContainer CookieContainer { get; private set; }

        [SecuritySafeCritical]
        public ExtendedWebClient()
        {
            CookieContainer = new CookieContainer();
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);

            if (request is HttpWebRequest)
                (request as HttpWebRequest).CookieContainer = CookieContainer;

            return request;
        }

        public void Post(string address, string parameters, Action<string> onResponseGot)
        {
            var uri = new Uri(address);
            var r = (HttpWebRequest)WebRequest.Create(uri);
            r.Method = "POST";

            r.BeginGetRequestStream(delegate(IAsyncResult req)
            {
                var outStream = r.EndGetRequestStream(req);

                using (var w = new StreamWriter(outStream))
                    w.Write(parameters);

                r.BeginGetResponse(delegate(IAsyncResult result)
                {
                    try
                    {
                        var response = (HttpWebResponse)r.EndGetResponse(result);

                        using (var stream = response.GetResponseStream())
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                onResponseGot(reader.ReadToEnd());
                            }
                        }
                    }
                    catch
                    {
                        onResponseGot(null);
                    }

                }, null);

            }, null);
        }
    }
}
