using GolfClub.Model;
using GolfClub.Utilities;
using System.ComponentModel;
using System.Windows;

namespace GolfClub.Windows
{
    /// <summary>
    /// Interaction logic for EditPersonWindow.xaml
    /// </summary>
    public partial class EditPersonWindow
    {
        #region Constructors

        public EditPersonWindow(Person person)
        {
            InitializeComponent();
            this.LoadWindowSettings();
            DataContext = person;
        }

        #endregion Constructors

        #region Methods

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            this.SaveWindowSettings();
        }

        #endregion Methods
    }
}