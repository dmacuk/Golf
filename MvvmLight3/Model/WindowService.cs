using GolfClub.Windows;
using System.Collections.Generic;
using System.Windows;

namespace GolfClub.Model
{
    public class WindowService : IWindowService
    {
        #region Methods

        public bool EditPerson(Person person)
        {
            return new EditPersonWindow(person).ShowDialog() == true;
        }

        public void LaunchEmailSetupWindow()
        {
            new EmailSetup().ShowDialog();
        }

        public void LaunchEmailWindow(List<string> persons)
        {
            Window win = new Emailer(persons);
            win.ShowDialog();
        }

        public void PlayGame(Person person)
        {
            new PlayGameWindow(person).ShowDialog();
        }

        public bool? RenewMembership(Person person)
        {
            return new RenewMembershipWindow(person).ShowDialog();
        }

        #endregion Methods
    }
}