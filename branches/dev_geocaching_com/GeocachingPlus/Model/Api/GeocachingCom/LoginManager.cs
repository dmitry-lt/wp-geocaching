using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace GeocachingPlus.Model.Api.GeocachingCom
{
    public class LoginManager
    {
        /**
         * Check if the user has been logged in when he retrieved the data.
         *
         * @param page
         * @return <code>true</code> if user is logged in, <code>false</code> otherwise
         */
        public static bool GetLoginStatus(string page) {
            if (String.IsNullOrWhiteSpace(page)) {
//                Log.e("Login.checkLogin: No page given");
                return false;
            }

//            setActualStatus(cgeoapplication.getInstance().getString(R.string.init_login_popup_ok));

            // on every page except login page
            if (BaseUtils.Matches(page, GCConstants.PATTERN_LOGIN_NAME))
            {
                return true;
            }
/*
            setActualLoginStatus(BaseUtils.matches(page, GCConstants.PATTERN_LOGIN_NAME));
            if (isActualLoginStatus()) {
                setActualUserName(BaseUtils.getMatch(page, GCConstants.PATTERN_LOGIN_NAME, true, "???"));
                setActualCachesFound(Integer.parseInt(BaseUtils.getMatch(page, GCConstants.PATTERN_CACHES_FOUND, true, "0").replaceAll("[,.]", "")));
                Settings.setMemberStatus(BaseUtils.getMatch(page, GCConstants.PATTERN_MEMBER_STATUS, true, null));
                if ( page.contains(GCConstants.MEMBER_STATUS_RENEW) ) {
                    Settings.setMemberStatus(GCConstants.MEMBER_STATUS_PM);
                }
                return true;
            }
*/

            // login page
            if (BaseUtils.Matches(page, GCConstants.PATTERN_LOGIN_NAME_LOGIN_PAGE))
            {
                return true;
            }
/*
            setActualLoginStatus(BaseUtils.matches(page, GCConstants.PATTERN_LOGIN_NAME_LOGIN_PAGE));
            if (isActualLoginStatus()) {
                setActualUserName(Settings.getUsername());
                // number of caches found is not part of this page
                return true;
            }
*/

//            setActualStatus(cgeoapplication.getInstance().getString(R.string.init_login_popup_failed));
            return false;
        }

        /**
         * read all viewstates from page
         *
         * @return string[] with all view states
         */
        public string[] GetViewstates(string page) {
            // Get the number of viewstates.
            // If there is only one viewstate, __VIEWSTATEFIELDCOUNT is not present

            if (page == null) { // no network access
                return null;
            }

            // TODO: not sure if viewstates are matched correctly
            var count = 1;
            var matcherViewstateCount = GCConstants.ViewstateFieldCountRegex.Match(page);
            if (matcherViewstateCount.Groups.Count > 0) {
                try {
                    count = Convert.ToInt32(matcherViewstateCount.Groups[1].Value);
                } catch (Exception e) {
//                    Log.e("getViewStates", e);
                }
            }

            var viewstates = new string[count];

            // Get the viewstates
            var matcherViewstates = GCConstants.ViewstatesRegex.Matches(page);
            foreach (Match mvs in matcherViewstates)
            {
                var sno = mvs.Groups[1].Value; // number of viewstate
                int no;
                if (sno.Length == 0) {
                    no = 0;
                }
                else {
                    try {
                        no = Convert.ToInt32(sno);
                    } catch (Exception e) {
//                        Log.e("getViewStates", e);
                        no = 0;
                    }
                }
                viewstates[no] = mvs.Groups[2].Value;
            }

            if (viewstates.Length != 1 || viewstates[0] != null) {
                return viewstates;
            }

            // no viewstates were present
            return null;
        }

        private void Login(Action<StatusCode> processResult, string username, string password, bool retry) {
            var login = new ImmutablePair<string, string>(username, password);

            if (login == null || String.IsNullOrWhiteSpace(login.left) || String.IsNullOrWhiteSpace(login.right)) {
//                Login.setActualStatus(cgeoapplication.getInstance().getString(R.string.err_login));
//                Log.e("Login.login: No login information stored");
                processResult(StatusCode.NO_LOGIN_INFO_STORED);
                return;
            }

//            Login.setActualStatus(cgeoapplication.getInstance().getString(R.string.init_login_popup_working));
            var client = new WebClient();

            client.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    processResult(StatusCode.CONNECTION_FAILED);
                    return;
                }

                var loginData = e.Result;
//                if (e.loginResponse != null && loginResponse.getStatusLine().getStatusCode() == 503 && BaseUtils.matches(loginData, GCConstants.PATTERN_MAINTENANCE)) {
//                    return StatusCode.MAINTENANCE;
//                }

                if (String.IsNullOrWhiteSpace(loginData)) {
//                    Log.e("Login.login: Failed to retrieve login page (1st)");
                    processResult(StatusCode.CONNECTION_FAILED); // no loginpage
                    return;
                }

                if (GetLoginStatus(loginData)) {
//                    Log.i("Already logged in Geocaching.com as " + login.left + " (" + Settings.getMemberStatus() + ')');
                    // TODO: do we need to switch to English?
//                    Login.switchToEnglish(loginData);
                    processResult(StatusCode.NO_ERROR); // logged in
                    return;
                }

//                TODO:
/*
                Cookies.clearCookies();
                Settings.setCookieStore(null);
*/
                                    
                var parameters = new Dictionary<string, string>()
                    {
                        {"__EVENTTARGET", ""},
                        {"__EVENTARGUMENT", ""},
                        {"ctl00$ContentBody$tbUsername", login.left},
                        {"ctl00$ContentBody$tbPassword", login.right},
                        {"ctl00$ContentBody$cbRememberMe", "on"},
                        {"ctl00$ContentBody$btnSignIn", "Login"},
                    };

                var viewstates = GetViewstates(loginData);
                
                // TODO:
/*
                if (isEmpty(viewstates)) {
                    Log.e("Login.login: Failed to find viewstates");
                    return StatusCode.LOGIN_PARSE_ERROR; // no viewstates
                }
                Login.putViewstates(params, viewstates);

                loginResponse = Network.postRequest("https://www.geocaching.com/login/default.aspx", params);
                loginData = Network.getResponseData(loginResponse);

                if (StringUtils.isBlank(loginData)) {
                    Log.e("Login.login: Failed to retrieve login page (2nd)");
                    // FIXME: should it be CONNECTION_FAILED to match the first attempt?
                    return StatusCode.COMMUNICATION_ERROR; // no login page
                }

                if (Login.getLoginStatus(loginData)) {
                    Log.i("Successfully logged in Geocaching.com as " + login.left + " (" + Settings.getMemberStatus() + ')');

                    Login.switchToEnglish(loginData);
                    Settings.setCookieStore(Cookies.dumpCookieStore());

                    return StatusCode.NO_ERROR; // logged in
                }

                if (loginData.contains("Your username/password combination does not match.")) {
                    Log.i("Failed to log in Geocaching.com as " + login.left + " because of wrong username/password");
                    return StatusCode.WRONG_LOGIN_DATA; // wrong login
                }

                Log.i("Failed to log in Geocaching.com as " + login.left + " for some unknown reason");
                if (retry) {
                    Login.switchToEnglish(loginData);
                    return login(false);
                }

                return StatusCode.UNKNOWN_ERROR; // can't login
*/
            };
            
            client.DownloadStringAsync(new Uri("https://www.geocaching.com/login/default.aspx"));
        }

        public void Login(Action<StatusCode> processResult, string username, string password)
        {
            Login(processResult, username, password, true);
        }

    }
}
