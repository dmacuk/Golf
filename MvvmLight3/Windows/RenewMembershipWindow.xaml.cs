using GolfClub.Model;
using GolfClub.Utilities;
using System;
using System.ComponentModel;
using System.Windows;

namespace GolfClub.Windows
{
    public partial class RenewMembershipWindow
    {
        #region Constructors

        public RenewMembershipWindow(Person person)
        {
            InitializeComponent();
            this.LoadWindowSettings();
            if (DateTime.Now > person.MembershipExpiryDate)
            {
                person.MembershipStartDate = DateTime.Now;
            }
            DataContext = person;
        }

        #endregion Constructors

        #region Methods

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            this.SaveWindowSettings();
        }

        #endregion Methods
    }
}