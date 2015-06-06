using System.Collections.Generic;
using System.Windows;
using GolfClub.Interfaces;
using GolfClub.Model;
using GolfClub.Windows;
using Microsoft.Win32;

namespace GolfClub.Services
{
    public class WindowService : IWindowService
    {
        #region Methods

        public bool EditPerson(Person person)
        {
            return new EditPersonWindow(person).ShowDialog() == true;
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

        public string GetAttachment()
        {
            var dlg = new OpenFileDialog {Filter = "All Files (*.*)|*.*"};

            if (dlg.ShowDialog() == true)
            {
                return dlg.FileName;
            }
            else
            {
                return string.Empty;
            }
                ;

        }

        public void LaunchSettingsWindow()
        {
            new SettingsWindow().ShowDialog();
        }

        public void Report(string reportTitle, List<Person> data)
        {
            new ReportWindow(reportTitle, data).ShowDialog();
        }

        #endregion Methods
    }
}