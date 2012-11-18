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

                    //TODO: this one works :)
                    post =
                        "__EVENTTARGET=&__EVENTARGUMENT=&ctl00%24ContentBody%24tbUsername=remcont&ctl00%24ContentBody%24tbPassword=qwerty&ctl00%24ContentBody%24cbRememberMe=on&ctl00%24ContentBody%24btnSignIn=Login&__VIEWSTATE=%2FwEPDwUKMTIzNDE2NTc3MQ8WAh4OTG9naW4uUmVkaXJlY3RlFgJmD2QWBGYPZBYEAgoPFgIeBFRleHQFYjxtZXRhIG5hbWU9IkNvcHlyaWdodCIgY29udGVudD0iQ29weXJpZ2h0IChjKSAyMDAwLTIwMTIgR3JvdW5kc3BlYWssIEluYy4gQWxsIFJpZ2h0cyBSZXNlcnZlZC4iIC8%2BZAILDxYCHwEFRzwhLS0gQ29weXJpZ2h0IChjKSAyMDAwLTIwMTIgR3JvdW5kc3BlYWssIEluYy4gQWxsIFJpZ2h0cyBSZXNlcnZlZC4gLS0%2BZAIBD2QWCgIKDxYCHgdWaXNpYmxlZ2QCKw9kFgQCAw8WAh8BBQdFbmdsaXNoZAIFDxYCHgtfIUl0ZW1Db3VudAIQFiBmD2QWAgIBDw8WCB4PQ29tbWFuZEFyZ3VtZW50BQVlbi1VUx4LQ29tbWFuZE5hbWUFDVNldFRlbXBMb2NhbGUfAQUHRW5nbGlzaB4QQ2F1c2VzVmFsaWRhdGlvbmhkZAIBD2QWAgIBDw8WCB8EBQVkZS1ERR8FBQ1TZXRUZW1wTG9jYWxlHwEFB0RldXRzY2gfBmhkZAICD2QWAgIBDw8WCB8EBQVmci1GUh8FBQ1TZXRUZW1wTG9jYWxlHwEFCUZyYW7Dp2Fpcx8GaGRkAgMPZBYCAgEPDxYIHwQFBXB0LVBUHwUFDVNldFRlbXBMb2NhbGUfAQUKUG9ydHVndcOqcx8GaGRkAgQPZBYCAgEPDxYIHwQFBWNzLUNaHwUFDVNldFRlbXBMb2NhbGUfAQUJxIxlxaF0aW5hHwZoZGQCBQ9kFgICAQ8PFggfBAUFc3YtU0UfBQUNU2V0VGVtcExvY2FsZR8BBQdTdmVuc2thHwZoZGQCBg9kFgICAQ8PFggfBAUFZXMtRVMfBQUNU2V0VGVtcExvY2FsZR8BBQhFc3Bhw7FvbB8GaGRkAgcPZBYCAgEPDxYIHwQFBWl0LUlUHwUFDVNldFRlbXBMb2NhbGUfAQUISXRhbGlhbm8fBmhkZAIID2QWAgIBDw8WCB8EBQVubC1OTB8FBQ1TZXRUZW1wTG9jYWxlHwEFCk5lZGVybGFuZHMfBmhkZAIJD2QWAgIBDw8WCB8EBQVjYS1FUx8FBQ1TZXRUZW1wTG9jYWxlHwEFB0NhdGFsw6AfBmhkZAIKD2QWAgIBDw8WCB8EBQVwbC1QTB8FBQ1TZXRUZW1wTG9jYWxlHwEFBlBvbHNraR8GaGRkAgsPZBYCAgEPDxYIHwQFBWV0LUVFHwUFDVNldFRlbXBMb2NhbGUfAQUFRWVzdGkfBmhkZAIMD2QWAgIBDw8WCB8EBQVuYi1OTx8FBQ1TZXRUZW1wTG9jYWxlHwEFDk5vcnNrLCBCb2ttw6VsHwZoZGQCDQ9kFgICAQ8PFggfBAUFa28tS1IfBQUNU2V0VGVtcExvY2FsZR8BBQntlZzqta3slrQfBmhkZAIOD2QWAgIBDw8WCB8EBQVodS1IVR8FBQ1TZXRUZW1wTG9jYWxlHwEFBk1hZ3lhch8GaGRkAg8PZBYCAgEPDxYIHwQFBXJvLVJPHwUFDVNldFRlbXBMb2NhbGUfAQUIUm9tw6JuxIMfBmhkZAIuDw9kFgIeBWNsYXNzBQdzcGFuLTIwZAIvDxYCHwcFC3NwYW4tNCBsYXN0FgICAQ9kFgICAQ8PFgIfAQWCBDxpZnJhbWUgdHlwZT0iaWZyYW1lIiBzcmM9Imh0dHBzOi8vYWRzLmdyb3VuZHNwZWFrLmNvbS9hLmFzcHg%2FWm9uZUlEPTkmVGFzaz1HZXQmU2l0ZUlEPTEmWD0nZDgzZDMzMzg4NDYwNDlhMGJhODk4ZjRjYzk4MmM3NzAnIiB3aWR0aD0iMTIwIiBoZWlnaHQ9IjI0MCIgTWFyZ2lud2lkdGg9IjAiIE1hcmdpbmhlaWdodD0iMCIgSHNwYWNlPSIwIiBWc3BhY2U9IjAiIEZyYW1lYm9yZGVyPSIwIiBTY3JvbGxpbmc9Im5vIiBzdHlsZT0id2lkdGg6MTIwcHg7SGVpZ2h0OjI0MHB4OyI%2BPGEgaHJlZj0iaHR0cHM6Ly9hZHMuZ3JvdW5kc3BlYWsuY29tL2EuYXNweD9ab25lSUQ9OSZUYXNrPUNsaWNrJjtNb2RlPUhUTUwmU2l0ZUlEPTEiIHRhcmdldD0iX2JsYW5rIj48aW1nIHNyYz0iaHR0cHM6Ly9hZHMuZ3JvdW5kc3BlYWsuY29tL2EuYXNweD9ab25lSUQ9OSZUYXNrPUdldCZNb2RlPUhUTUwmU2l0ZUlEPTEiIHdpZHRoPSIxMjAiIGhlaWdodD0iMjQwIiBib3JkZXI9IjAiIGFsdD0iIiAvPjwvYT48L2lmcmFtZT5kZAIwD2QWBAIDDxYCHwEFB0VuZ2xpc2hkAgUPFgIfAwIQFiBmD2QWAgIBDw8WCB8EBQVlbi1VUx8FBQ1TZXRUZW1wTG9jYWxlHwEFB0VuZ2xpc2gfBmhkZAIBD2QWAgIBDw8WCB8EBQVkZS1ERR8FBQ1TZXRUZW1wTG9jYWxlHwEFB0RldXRzY2gfBmhkZAICD2QWAgIBDw8WCB8EBQVmci1GUh8FBQ1TZXRUZW1wTG9jYWxlHwEFCUZyYW7Dp2Fpcx8GaGRkAgMPZBYCAgEPDxYIHwQFBXB0LVBUHwUFDVNldFRlbXBMb2NhbGUfAQUKUG9ydHVndcOqcx8GaGRkAgQPZBYCAgEPDxYIHwQFBWNzLUNaHwUFDVNldFRlbXBMb2NhbGUfAQUJxIxlxaF0aW5hHwZoZGQCBQ9kFgICAQ8PFggfBAUFc3YtU0UfBQUNU2V0VGVtcExvY2FsZR8BBQdTdmVuc2thHwZoZGQCBg9kFgICAQ8PFggfBAUFZXMtRVMfBQUNU2V0VGVtcExvY2FsZR8BBQhFc3Bhw7FvbB8GaGRkAgcPZBYCAgEPDxYIHwQFBWl0LUlUHwUFDVNldFRlbXBMb2NhbGUfAQUISXRhbGlhbm8fBmhkZAIID2QWAgIBDw8WCB8EBQVubC1OTB8FBQ1TZXRUZW1wTG9jYWxlHwEFCk5lZGVybGFuZHMfBmhkZAIJD2QWAgIBDw8WCB8EBQVjYS1FUx8FBQ1TZXRUZW1wTG9jYWxlHwEFB0NhdGFsw6AfBmhkZAIKD2QWAgIBDw8WCB8EBQVwbC1QTB8FBQ1TZXRUZW1wTG9jYWxlHwEFBlBvbHNraR8GaGRkAgsPZBYCAgEPDxYIHwQFBWV0LUVFHwUFDVNldFRlbXBMb2NhbGUfAQUFRWVzdGkfBmhkZAIMD2QWAgIBDw8WCB8EBQVuYi1OTx8FBQ1TZXRUZW1wTG9jYWxlHwEFDk5vcnNrLCBCb2ttw6VsHwZoZGQCDQ9kFgICAQ8PFggfBAUFa28tS1IfBQUNU2V0VGVtcExvY2FsZR8BBQntlZzqta3slrQfBmhkZAIOD2QWAgIBDw8WCB8EBQVodS1IVR8FBQ1TZXRUZW1wTG9jYWxlHwEFBk1hZ3lhch8GaGRkAg8PZBYCAgEPDxYIHwQFBXJvLVJPHwUFDVNldFRlbXBMb2NhbGUfAQUIUm9tw6JuxIMfBmhkZBgBBR5fX0NvbnRyb2xzUmVxdWlyZVBvc3RCYWNrS2V5X18WAQUeY3RsMDAkQ29udGVudEJvZHkkY2JSZW1lbWJlck1l2pY%2BN5DfVSSshBLQGyeV6epepKA%3D";

                    try
                    {
                        var req = (HttpWebRequest)asynchronousResult.AsyncState;

                        // TODO:
                        req.Headers["Accept-Charset"] = "utf-8,iso-8859-1;q=0.8,utf-16;q=0.8,*;q=0.7";
                        req.Headers["Accept-Language"] = "en-US,*;q=0.9";
                        req.Headers["X-Requested-With"] = "XMLHttpRequest";
                        req.ContentType = "application/x-www-form-urlencoded";
                        req.Headers["User-Agent"] = "Mozilla/5.0 (X11; Linux x86_64; rv:9.0.1) Gecko/20100101 Firefox/9.0.1";
                        req.Headers["Accept-Encoding"] = "gzip";

                        // TODO: ???
                        req.Headers["Host"] = "www.geocaching.com";
                        req.Headers["Origin"] = "http://www.geocaching.com";
                        req.Headers["Referer"] = "https://www.geocaching.com/";

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
