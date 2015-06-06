using System;
using System.Collections.Generic;
using System.IO;
using GolfClub.Design;
using GolfClub.Interfaces;
using GolfClub.Model;
using Newtonsoft.Json;

namespace GolfClub.Services
{
    public class DataService : IDataService
    {
        #region Fields

        private readonly string _dataFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "GolfClub.json");

        #endregion Fields

        #region Methods

        public void Load(Action<List<Person>, Exception> callback)
        {
            List<Person> people;
            Exception exception;
            if (!File.Exists(_dataFile))
            {
                people = DesignDataService.BuildDummyPeople();
                exception = null;
            }
            else
            {
                try
                {
                    var json = File.ReadAllText(_dataFile);
                    people = JsonConvert.DeserializeObject<List<Person>>(json);
                    exception = null;
                }
                catch (Exception e)
                {
                    people = null;
                    exception = e;
                }
            }
            callback(people, exception);
        }

        public void Save(List<Person> people)
        {
            var json = JsonConvert.SerializeObject(people);
            File.WriteAllText(_dataFile, json);
        }

        #endregion Methods
    }
}