using System;
using System.IO;
using System.Net;
using System.Web;

namespace GeocachingComConsoleApp
{
    class Program
    {
        private static string ExtractViewState(string s)
        {
            var viewStateNameDelimiter = "__VIEWSTATE";
            var valueDelimiter = "value=\"";

            var viewStateNamePosition = s.IndexOf(viewStateNameDelimiter);
            var viewStateValuePosition = s.IndexOf(valueDelimiter, viewStateNamePosition);

            var viewStateStartPosition = viewStateValuePosition + valueDelimiter.Length;
            var viewStateEndPosition = s.IndexOf("\"", viewStateStartPosition);

            return HttpUtility.UrlEncodeUnicode(s.Substring(viewStateStartPosition, viewStateEndPosition - viewStateStartPosition));
        }

        static void Main(string[] args)
        {
            var username = "test";
            var password = "test";

            // first, request the login form to get the viewstate value
            var webRequest = (HttpWebRequest)WebRequest.Create("https://www.geocaching.com/login/default.aspx");
            var responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
            var responseData = responseReader.ReadToEnd();
            responseReader.Close();

            // extract the viewstate value and build out POST data
            var viewState = ExtractViewState(responseData);

            var postData =
                  String.Format(
                     "__VIEWSTATE={0}&ctl00$ContentBody$tbUsername={1}&ctl00$ContentBody$tbPassword={2}&ctl00$ContentBody$btnSignIn=Login&ctl00$ContentBody$cbRememberMe=on",
                     viewState, username, password
                  );

            // have a cookie container ready to receive the forms auth cookie
            CookieContainer cookies = new CookieContainer();

            // now post to the login form
            webRequest = WebRequest.Create("https://www.geocaching.com/login/default.aspx") as HttpWebRequest;
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.CookieContainer = cookies;

            // write the form values into the request message
            StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream());
            requestWriter.Write(postData);
            requestWriter.Close();

            responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());

            // and read the response
            responseData = responseReader.ReadToEnd();
            responseReader.Close();

            var success = responseData.Contains(username);

            Console.WriteLine(success);
            Console.ReadLine();
        }
    }
}
