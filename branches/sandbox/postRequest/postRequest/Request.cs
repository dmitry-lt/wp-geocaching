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
using System.Collections.Generic;
using System.IO;

namespace postRequest
{
    public class Request
    {
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
                        var postBytes = new System.Text.UTF8Encoding().GetBytes(post); // change to utf8 from ascii

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
                                    //if ("gzip".Equals(response.Headers["Content-Encoding"]))
                                    //{
                                    //  streamResponse = new GZipInputStream(response.GetResponseStream());
                                    //  }
                                    // else
                                    // {
                                    streamResponse = response.GetResponseStream();
                                    //}
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
                            catch (WebException ex)
                            {
                                onResponseGot(null);
                            }
                        };

                        // Start the asynchronous operation to get the response
                        req.BeginGetResponse(getResponseCallback, req);
                    }
                    catch (Exception ex)
                    {
                        onResponseGot(null);
                    }
                };

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(address);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";

                // start the asynchronous operation
                httpWebRequest.BeginGetRequestStream(new AsyncCallback(getRequestStreamCallback), httpWebRequest);
            }
            catch (Exception ex)
            {

            }
        }

        public void Post(string address, Dictionary<string, string> parameters, Action<string> onResponseGot)
        {
            var stringParams = FormUrlParameterQuery(parameters);
            Post(address, stringParams, onResponseGot);
        }

        public static string FormUrlParameterQuery(Dictionary<string, string> parameters)
        {
            var result = "";
            var firstParameter = true;
            foreach (var k in parameters.Keys)
            {
                if (!firstParameter)
                {
                    result += "&";
                }
                result += k + "=" + parameters[k];
                firstParameter = false;
            }
            return result;
        }
    }
}
