using System.Windows.Input;

namespace GeocachingPlus.ViewModel
{
    public class GeocachingComLoginPageViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public ICommand LoginCommand { get; set; }
    }
}
