using System;
using System.Windows.Input;
using GeocachingPlus.Model;
using GeocachingPlus.Model.Api.GeocachingCom;

namespace GeocachingPlus.ViewModel
{
    public class GeocachingComLoginPageViewModel
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
                            _settings.GeocachingComLogin = Username;
                            _settings.GeocachingComPassword = Password;
                            
                            //TODO: implement
                            Action<StatusCode> processResult = sc => { };

                            _loginManager.Login(processResult, Username, Password);
                        }
                );
            }
        }

        public GeocachingComLoginPageViewModel()
        {
             Username = _settings.GeocachingComLogin;
             Password = _settings.GeocachingComPassword;
        }
    }
}
