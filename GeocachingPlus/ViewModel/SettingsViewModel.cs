using System.Windows.Input;
using Microsoft.Phone.Tasks;
using GeocachingPlus.Model;
using GeocachingPlus.Model.Dialogs;
using GeocachingPlus.Resources.Localization;

namespace GeocachingPlus.ViewModel
{
    public class SettingsViewModel : BaseViewModel
    {
        private Settings settings;
        private bool isLocationEnabled;

        public string SupportEmail { get { return Support.Email; } }

        public ICommand SendEmailCommand { get; private set; }
        public ICommand ChooseRoadModeCommand { get; private set; }
        public ICommand ChooseAerialModeCommand { get; private set; }
        public ICommand ShowPrivacyStatementDialogCommand { get; private set; }
        public bool IsAerialChecked { get; private set; }
        public bool IsRoadChecked { get; private set; }

        public ICommand ReviewCommand { get { return new ButtonCommand(o => new MarketplaceReviewTask().Show()); } }

        public bool IsLocationEnabled
        {
            get
            {
                return isLocationEnabled;
            }
            set
            {
                isLocationEnabled = value;
                settings.IsLocationEnabled = value;
                RaisePropertyChanged(() => IsLocationEnabled);
            }
        }

        public SettingsViewModel()
        {
            settings = new Settings();

            SendEmailCommand = new ButtonCommand(SendEmail);
            ChooseRoadModeCommand = new ButtonCommand(ChooseRoadMode);
            ChooseAerialModeCommand = new ButtonCommand(ChooseAerialMode);
            ShowPrivacyStatementDialogCommand = new ButtonCommand(ShowPrivacyStatementDialog);

            if (settings.MapMode == MapMode.Road)
            {
                IsRoadChecked = true;
            }
            else
            {
                IsAerialChecked = true;
            }

            IsLocationEnabled = settings.IsLocationEnabled;
        }

        private void SendEmail(object p)
        {
            var emailComposeTask = new EmailComposeTask
            {
                Subject = AppResources.EmailSubject,
                To = Support.Email
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

        private void ShowPrivacyStatementDialog(object p)
        {
            PrivacyStatementDialog.Show();
        }

    }
}
