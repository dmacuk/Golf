using GolfClub.Properties;
using GolfClub.Utilities;
using GolfClub.ViewModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace GolfClub.Windows
{
    /// <summary>
    /// Interaction logic for Emailer.xaml
    /// </summary>
    public partial class Emailer
    {
        #region Fields

        private readonly Settings _settings = Settings.Default;

        #endregion Fields

        #region Constructors

        public Emailer()
        {
            InitializeComponent();
            this.LoadWindowSettings();
        }

        public Emailer(List<string> persons)
            : this()
        {
            var model = ((EmailViewModel)DataContext);
            model.EmailAddresses = persons;

            model.FromAddress = _settings.FromAddress;
            model.Password = _settings.Password;
            model.Smtp = _settings.SMTP;
            model.User = _settings.User;
        }

        #endregion Constructors

        #region Methods

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            this.SaveWindowSettings();
        }

        #endregion Methods

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SendClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}