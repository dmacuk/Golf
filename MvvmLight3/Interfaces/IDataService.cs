using System;
using System.Collections.Generic;
using GolfClub.Model;

namespace GolfClub.Interfaces
{
    public interface IDataService
    {
        #region Methods

        void Load(Action<List<Person>, Exception> callback);

        void Save(List<Person> people);

        #endregion Methods
    }
}