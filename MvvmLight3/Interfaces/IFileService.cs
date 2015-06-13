using System.Collections.Generic;
using GolfClub.Model;

namespace GolfClub.Design
{
    public interface IFileService
    {
        void WriteCvFile(IEnumerable<Person> data);
    }
}
