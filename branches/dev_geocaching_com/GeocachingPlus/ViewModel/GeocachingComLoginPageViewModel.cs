using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using GeocachingPlus.Model;
using GeocachingPlus.Model.Api.GeocachingCom;

namespace GeocachingPlus.ViewModel
{
    public class GeocachingComLoginPageViewModel : BaseViewModel
    {
        private readonly Settings _settings = new Settings();
        private readonly LoginManager _loginManager = new LoginManager();

        public string Username { get; set; }
        public string Password { get; set; }
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
                            
                            //TODO: implement
                            Action<StatusCode> processResult = 
                                sc =>
                                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                                    {
                                        Loading = false;
                                        if (sc == StatusCode.NO_ERROR)
                                        {
                                            if (null != LoginSucceeded)
                                            {
                                                LoginSucceeded(this, new EventArgs());
                                            }
                                        }
                                        else
                                        {
                                            LoginFailed = true;
                                        }
                                    });

                            _loginManager.Login(processResult, Username, Password);
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
             Username = _settings.GeocachingComLogin;
             Password = _settings.GeocachingComPassword;
        }
    }
}
