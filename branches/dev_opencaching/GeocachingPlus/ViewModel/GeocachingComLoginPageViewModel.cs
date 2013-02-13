using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using GeocachingPlus.Model;
using GeocachingPlus.Model.Api.GeocachingCom;

namespace GeocachingPlus.ViewModel
{
    public class GeocachingComLoginPageViewModel : BaseViewModel
    {
        private readonly Settings _settings = new Settings();
        private readonly LoginManager _loginManager = new LoginManager();

        public bool _loggedIn;
        public bool LoggedIn
        {
            get { return _loggedIn; }
            set
            {
                _loggedIn = value;
                RaisePropertyChanged(() => LoggedIn);
                _settings.GeocachingComLoggedIn = value;
            }
        }

        private string _username;
        public string Username
        {
            get { return _username; } 
            set 
            { 
                _username = value;
                RaisePropertyChanged(() => Username);
            }
        }
        
        public string Password { get; set; }

        public void Login()
        {
            Login(null, null);
        }

        public void Login(Action<object> succeededCallback, Action<object> failedCallback)
        {
            Action<StatusCode> processResult =
                sc =>
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        Loading = false;
                        if (sc == StatusCode.NO_ERROR)
                        {
                            LoggedIn = true;
                            if (null != LoginSucceeded)
                            {
                                LoginSucceeded(this, new EventArgs());
                            }
                            if (null != succeededCallback)
                            {
                                succeededCallback(null);
                            }
                        }
                        else
                        {
                            LoggedIn = false;
                            LoginFailed = true;
                            if (null != failedCallback)
                            {
                                failedCallback(null);
                            }
                        }
                    });

            _loginManager.Login(processResult, Username, Password);
        }

        public ICommand LoginCommand
        {
            get
            {
                return new ButtonCommand(
                    o =>
                        {
                            // logout first
                            _loginManager.Logout();

                            Loading = true;
                            LoginFailed = false;

                            _settings.GeocachingComLogin = Username;
                            _settings.GeocachingComPassword = Password;

                            Login();
                        }
                );
            }
        }

        private bool _loading;
        public bool Loading
        {
            get { return _loading; }
            set
            {
                _loading = value;
                RaisePropertyChanged(() => Loading);
            }
        }

        private bool _loginFailed;
        public bool LoginFailed
        {
            get { return _loginFailed; }
            set
            {
                _loginFailed = value;
                RaisePropertyChanged(() => LoginFailed);
            }
        }

        public event EventHandler LoginSucceeded;

        public GeocachingComLoginPageViewModel()
        {
            // don't initialize settings if in design view
            if (DesignerProperties.IsInDesignTool) return;

            Username = _settings.GeocachingComLogin;
            Password = _settings.GeocachingComPassword;
            _loggedIn = _settings.GeocachingComLoggedIn;
        }
    }
}
