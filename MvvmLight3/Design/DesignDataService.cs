using GolfClub.Interfaces;
using GolfClub.Model;
using System;
using System.Collections.Generic;

namespace GolfClub.Design
{
    public class DesignDataService : IDataService
    {
        #region Methods

        public static List<Person> BuildDummyPeople()
        {
            var people = new List<Person>
            {
                new Person {Id = 1, MembershipNumber = "12345", Address = "Address 1", Email = "email 1", Phone = "phone 1", Name = "David McCallum", MembershipStartDate = null},
                new Person {Id = 2, MembershipNumber = "123", Address = "Address 2", Email = "email 2", Phone = "phone 2", Name = "J. G. Butchart ", MembershipStartDate = new DateTime(14, 06, 11)},
                new Person {Id = 3, MembershipNumber = "456", Address = "Address 3", Email = "email 3", Phone = "phone 3", Name = "Angus Beaton", MembershipStartDate= new DateTime(14, 6, 17)},
                new Person {Id = 4, MembershipNumber = "789", Address = "Address 4", Email = "email 4", Phone = "phone 4", Name = "Phil Croall"},
                new Person {Id = 5, Address = "Address 8", Email = "email 8", Phone = "phone 8", Name = "Anne Frew", MembershipNumber = "memnum"}
            };
            return people;
        }

        public void Load(Action<List<Person>, Exception> callback)
        {
            var people = BuildDummyPeople();

            callback(people, null);
        }

        public void Save(List<Person> people)
        {
            // Nothing to do
        }

        #endregion Methods
    }
}