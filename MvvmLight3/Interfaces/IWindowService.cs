﻿using System.Collections.Generic;
using GolfClub.Model;

namespace GolfClub.Interfaces
{
    public interface IWindowService
    {
        #region Methods

        void LaunchEmailWindow(List<string> persons);

        #endregion Methods

        bool EditPerson(Person person);

        void PlayGame(Person person);

        bool? RenewMembership(Person person);
        string GetAttachment();
        void LaunchSettingsWindow();
        void Report(string reportTitle, List<Person> data);
    }
}