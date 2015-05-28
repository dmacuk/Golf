using GolfClub.Properties;
using GolfClub.Utilities;
using System.ComponentModel;
using System.Windows;

namespace GolfClub.Windows
{
    /// <summary>
    /// Interaction logic for EmailSetup.xaml
    /// </summary>
    public partial class EmailSetup
    {
        #region Fields

        private readonly Settings _settings = Settings.Default;

        #endregion Fields

        #region Constructors

        public EmailSetup()
        {
            InitializeComponent();
            this.LoadWindowSettings();
            TxtSmtp.Text = _settings.SMTP;
            TxtFromAddress.Text = _settings.FromAddress;
            TxtPassword.Password = _settings.Password;
            TxtUser.Text = _settings.User;
        }

        #endregion Constructors

        #region Methods

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            _settings.SMTP = TxtSmtp.Text;
            _settings.FromAddress = TxtFromAddress.Text;
            _settings.Password = TxtPassword.Password;
            _settings.User = TxtUser.Text;
            _settings.Save();
            Close();
        }

        #endregion Methods

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            this.SaveWindowSettings();
        }
    }
}