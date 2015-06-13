using System;
using CsvHelper;
using CsvHelper.Configuration;
using GolfClub.Design;
using GolfClub.Model;
using System.Collections.Generic;
using System.IO;

namespace GolfClub.Services
{
    public class FileService : IFileService
    {
        public void WriteCvFile(IEnumerable<Person> data)
        {
            using (TextWriter textWriter = new StreamWriter(@"e:\temp\data.csv"))
            {
                var writer = new CsvWriter(textWriter);
                writer.Configuration.RegisterClassMap<PersonMap>();
                writer.Configuration.TrimFields = true;
                writer.WriteRecords(data);
                
            }
        }
    }

    public sealed class PersonMap : CsvClassMap<Person>
    {
        public PersonMap()
        {
            Map(p => p.MembershipNumber);
            Map(p => p.Name);
            Map(p => p.Address);
            Map(p => p.Phone);
            Map(p => p.Email);
            Map(p => p.MembershipExpiryDate).TypeConverterOption("dd/MM/yyy");
        }
    }
}