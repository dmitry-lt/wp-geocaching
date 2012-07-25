using System.Windows.Input;
using Microsoft.Phone.Tasks;
using WP_Geocaching.Model;
using WP_Geocaching.Resources.Localization;

namespace WP_Geocaching.ViewModel
{
    public class SettingsViewModel : BaseMapViewModel
    {
        private Settings settings;

        public ICommand SendEmailCommand { get; private set; }
        public ICommand ChooseRoadModeCommand { get; private set; }
        public ICommand ChooseAerialModeCommand { get; private set; }
        public bool IsAerialChecked { get; private set; }
        public bool IsRoadChecked { get; private set; }

        public SettingsViewModel()
        {
            settings = new Settings();

            SendEmailCommand = new ButtonCommand(SendEmail);
            ChooseRoadModeCommand = new ButtonCommand(ChooseRoadMode);
            ChooseAerialModeCommand = new ButtonCommand(ChooseAerialMode);

            if (settings.MapMode == MapMode.Road)
            {
                IsRoadChecked = true;
            }
            else
            {
                IsAerialChecked = true;
            }
        }

        private void SendEmail(object p)
        {
            var emailComposeTask = new EmailComposeTask
            {
                Subject = AppResources.EmailSubject,
                To = AppResources.SupportEmail
            };

            emailComposeTask.Show();
        }

        private void ChooseRoadMode(object p)
        {
            settings.MapMode = MapMode.Road;
        }

        private void ChooseAerialMode(object p)
        {
            settings.MapMode = MapMode.Aerial;
        }
    }


}
