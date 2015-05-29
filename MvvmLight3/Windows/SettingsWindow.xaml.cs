using System.ComponentModel;
using System.Windows;
using GolfClub.Properties;
using GolfClub.Utilities;

namespace GolfClub.Windows
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow
    {
        public SettingsWindow()
        {
            InitializeComponent();
            Closing+=WindowClosing;
            this.LoadWindowSettings();
            var settings = Settings.Default;
            ChkToolIcons.IsChecked = settings.ShowToolbarIcons;
            ChkToolText.IsChecked = settings.ShowToolbarText;
            TxtSmtp.Text = settings.SMTP;
            TxtFromAddress.Text = settings.FromAddress;
            TxtPassword.Password = settings.Password;
            TxtUser.Text = settings.User;
            // Gather user input
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            this.SaveWindowSettings();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void OkClick(object sender, RoutedEventArgs e)
        {
            if (ChkToolIcons.IsChecked == false && ChkToolText.IsChecked == false)
            {
                MessageBox.Show(this, "Must have one toolbar option selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                TabControl.SelectedIndex = 0;
                return;

            }
            var settings = Settings.Default;
            // Toolbar tab
            settings.ShowToolbarIcons = (bool)(ChkToolIcons.IsChecked.HasValue ? ChkToolIcons.IsChecked : true);
            settings.ShowToolbarText = (bool)(ChkToolText.IsChecked.HasValue ? ChkToolText.IsChecked : true);

            settings.SMTP = TxtSmtp.Text;
            settings.FromAddress = TxtFromAddress.Text;
            settings.Password = TxtPassword.Password;
            settings.User = TxtUser.Text;
            Close();
        }
    }
}
