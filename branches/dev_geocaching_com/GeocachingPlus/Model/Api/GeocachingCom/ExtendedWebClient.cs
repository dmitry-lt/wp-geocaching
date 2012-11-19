using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security;
using System.Text;
using System.Windows.Navigation;
using ICSharpCode.SharpZipLib.GZip;

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

                        // End the operation
                        var postStream = req.EndGetRequestStream(asynchronousResult);

                        // Convert the string into a byte array.
                        var postBytes = new AsciiEncoding().GetBytes(post);

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
                                    Stream streamResponse;
                                    if ("gzip".Equals(response.Headers["Content-Encoding"]))
                                    {
                                        streamResponse = new GZipInputStream(response.GetResponseStream());
                                    }
                                    else
                                    {
                                        streamResponse = response.GetResponseStream();
                                    }
                                    using (streamResponse)
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
                var cookies = new CookieContainer();
                httpWebRequest.CookieContainer = cookies;

                // TODO:
//                        httpWebRequest.Headers["Accept-Charset"] = "utf-8,iso-8859-1;q=0.8,utf-16;q=0.8,*;q=0.7";
//                        httpWebRequest.Headers["Accept-Language"] = "en-US,*;q=0.9";
//                        httpWebRequest.Headers["X-Requested-With"] = "XMLHttpRequest";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
//                        httpWebRequest.Headers["User-Agent"] = "Mozilla/5.0 (X11; Linux x86_64; rv:9.0.1) Gecko/20100101 Firefox/9.0.1";
//                        httpWebRequest.Headers["Accept-Encoding"] = "gzip";

                // TODO: ???
//                        httpWebRequest.Headers["Host"] = "www.geocaching.com";
//                        httpWebRequest.Headers["Origin"] = "http://www.geocaching.com";
//                        httpWebRequest.Headers["Referer"] = "http://www.geocaching.com/";
                

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


    public class AsciiEncoding : System.Text.Encoding
    {
        public override int GetMaxByteCount(int charCount)
        {
            return charCount;
        }
        public override int GetMaxCharCount(int byteCount)
        {
            return byteCount;
        }
        public override int GetByteCount(char[] chars, int index, int count)
        {
            return count;
        }
        public override byte[] GetBytes(char[] chars)
        {
            return base.GetBytes(chars);
        }
        public override int GetCharCount(byte[] bytes)
        {
            return bytes.Length;
        }
        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            for (int i = 0; i < charCount; i++)
            {
                bytes[byteIndex + i] = (byte)chars[charIndex + i];
            }
            return charCount;
        }
        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            return count;
        }
        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            for (int i = 0; i < byteCount; i++)
            {
                chars[charIndex + i] = (char)bytes[byteIndex + i];
            }
            return byteCount;
        }
    }

}
