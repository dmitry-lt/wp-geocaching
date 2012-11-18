using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security;
using System.Text;
using System.Windows.Navigation;

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

        private static byte[] GetBytes(string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public void Post(string address, string parameters, Action<string> onResponseGot)
        {
            try
            {
                Action<IAsyncResult> getRequestStreamCallback = (IAsyncResult asynchronousResult) =>
                {
                    var post = parameters;

                    try
                    {
                        var req = (HttpWebRequest)asynchronousResult.AsyncState;

                        // TODO:
                        req.Headers["Accept-Charset"] = "utf-8,iso-8859-1;q=0.8,utf-16;q=0.8,*;q=0.7";
                        req.Headers["Accept-Language"] = "en-US,*;q=0.9";
                        req.Headers["X-Requested-With"] = "XMLHttpRequest";

                        // End the operation
                        var postStream = req.EndGetRequestStream(asynchronousResult);

                        // Convert the string into a byte array.
                        var postBytes = Encoding.UTF8.GetBytes(post);

                        // Write to the request stream.
                        postStream.Write(postBytes, 0, postBytes.Length);
                        postStream.Close();

                        AsyncCallback getResponseCallback = (IAsyncResult asynchResult) =>
                        {
                            var request = (HttpWebRequest)asynchResult.AsyncState;

                            // End the operation
                            try
                            {
                                using (var response = (HttpWebResponse)request.EndGetResponse(asynchResult))
                                {
                                    var rcode = response.StatusCode;
                                    using (var streamResponse = response.GetResponseStream())
                                    {
                                        using (var streamRead = new StreamReader(streamResponse))
                                        {
                                            var responseString = streamRead.ReadToEnd();
                                            onResponseGot(responseString);
                                        }
                                    }
                                }
                            }
                            catch (WebException e)
                            {
                                onResponseGot(null);
                            }
                        };

                        // Start the asynchronous operation to get the response
                        req.BeginGetResponse(getResponseCallback, req);
                    }
                    catch (Exception ex)
                    {

                    }
                };

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(address);
                httpWebRequest.Method = "POST";
                
                // start the asynchronous operation
                httpWebRequest.BeginGetRequestStream(new AsyncCallback(getRequestStreamCallback), httpWebRequest);
            }
            catch (Exception ex)
            {
            
            }
        }


/*
        public void Post(string address, string parameters, Action<string> onResponseGot)
        {
            var uri = new Uri(address);


            var getResponseCallback = 
                req =>
                    {
                        
                    }

            var request = (HttpWebRequest)WebRequest.Create(uri); ;
            request.Method = "POST";

            // End the operation
            Stream postStream = request.EndGetRequestStream(asynchronousResult);

            // Convert the string into a byte array.
            byte[] postBytes = Encoding.UTF8.GetBytes(post);

            // Write to the request stream.
            postStream.Write(postBytes, 0, postBytes.Length);
            postStream.Close();

            // Start the asynchronous operation to get the response
            request.BeginGetResponse(new AsyncCallback(getResponseCallback), request);

            r.BeginGetRequestStream(delegate(IAsyncResult req)
            {
                var outStream = r.EndGetRequestStream(req);

                var bytes = GetBytes(parameters);
                outStream.Write(bytes, 0, bytes.Length);
                outStream.Flush();

                r.BeginGetResponse(delegate(IAsyncResult result)
                {
                    try
                    {
                        var response = (HttpWebResponse) r.EndGetResponse(result);

                        using (var stream = response.GetResponseStream())
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                onResponseGot(reader.ReadToEnd());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        onResponseGot(null);
                    }

                }, null);

            }, null);
        }
*/

        public void Post(string address, Dictionary<string, string> parameters, Action<string> onResponseGot)
        {
            var stringParams = UrlHelper.FormUrlParameterQuery(parameters);
            Post(address, stringParams, onResponseGot);
        }
    }
}
