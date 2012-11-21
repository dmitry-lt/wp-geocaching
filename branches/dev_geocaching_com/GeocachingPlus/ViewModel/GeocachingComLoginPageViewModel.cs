using System;
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
                            Loading = true;

                            _settings.GeocachingComLogin = Username;
                            _settings.GeocachingComPassword = Password;
                            
                            //TODO: implement
                            Action<StatusCode> processResult = 
                                sc => 
                                    _dispatcher.BeginInvoke(() =>
                                    {
                                        Loading = false;
                                        if (sc == StatusCode.NO_ERROR)
                                        {
                                            if (null != LoginSuccess)
                                            {
                                                LoginSuccess(this, new EventArgs());
                                            }
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

        private readonly Dispatcher _dispatcher;

        public event EventHandler LoginSuccess;

        public GeocachingComLoginPageViewModel(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;

             Username = _settings.GeocachingComLogin;
             Password = _settings.GeocachingComPassword;
        }
    }
}
