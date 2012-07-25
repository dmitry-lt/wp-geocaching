using System.Windows.Input;
using Microsoft.Phone.Tasks;
using WP_Geocaching.Model;
using WP_Geocaching.Resources.Localization;

namespace WP_Geocaching.ViewModel
{
    public class SettingsViewModel : BaseMapViewModel
    {
        public ICommand SendEmailCommand { get; private set; }

        public SettingsViewModel()
        {
            SendEmailCommand = new ButtonCommand(SendEmail);
            
        }

        private void SendEmail(object p)
        {
            var emailComposeTask = new EmailComposeTask
            {
                Subject = "[Geocaching]",
                To = AppResources.SupportEmail
            };

            emailComposeTask.Show();
        }
    }


}
