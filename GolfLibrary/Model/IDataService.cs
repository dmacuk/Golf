using System;
using System.Collections.Generic;
using GolfLibrary.Model;

namespace GolfClub.Model
{
    public interface IDataService
    {
        #region Methods

        void Load(Action<List<Person>, Exception> callback);

        void Save(List<Person> people);

        #endregion Methods
    }
}