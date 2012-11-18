using System.Windows.Input;
using GeocachingPlus.Model;

namespace GeocachingPlus.ViewModel
{
    public class GeocachingComLoginPageViewModel
    {
        private readonly Settings _settings = new Settings();

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
