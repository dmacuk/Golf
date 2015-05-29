using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace GolfClub.Model
{
    public class Game : ObservableObject
    {
        #region Fields

        private DateTime _gameDate;
        private int _guests;

        #endregion Fields

        #region Constructors

        public Game()
        {
            GameDate = DateTime.Now;
            Guests = 0;
        }

        #endregion Constructors

        #region Properties

        public DateTime GameDate
        {
            get { return _gameDate; }
            set { Set("GameDate", ref _gameDate, value); }
        }

        public int Guests
        {
            get { return _guests; }
            set { Set("Guests", ref _guests, value); }
        }

        #endregion Properties
    }

    [Serializable]
    public class Person : ObservableObject
    {
        #region Fields

        private string _address;
        private string _email;
        private ObservableCollection<Game> _games = new ObservableCollection<Game>();
        private int _id;
        private DateTime? _membershipExpiryDate;
        private string _membershipNumber;
        private DateTime? _membershipStartDate;
        private string _name;
        private string _phone;
        private bool _selected;

        #endregion Fields

        #region Constructors

        public Person()
        {
        }

        public Person(Person person)
        {
            SetPerson(person);
        }

        #endregion Constructors

        #region Properties

        public string Address
        {
            get { return _address; }
            set { if (Set("Address", ref _address, value)) Dirty = true; }
        }

        [JsonIgnore]
        public bool AllowedGuests
        {
            get { return MembershipExpiryDate > DateTime.Now; }
        }

        public bool Dirty { get; private set; }

        public string Email
        {
            get { return _email; }
            set { if (Set("Email", ref _email, value)) Dirty = true; }
        }

        public ObservableCollection<Game> Games
        {
            get { return _games; }
            set { if (Set("Games", ref _games, value)) Dirty = true; }
        }

        public int Id
        {
            get { return _id; }
            set { if (Set("Id", ref _id, value)) Dirty = true; }
        }

        public DateTime? MembershipExpiryDate
        {
            get { return _membershipExpiryDate; }
            set { if (Set("MembershipExpiryDate", ref _membershipExpiryDate, value)) Dirty = true; }
        }

        public string MembershipNumber
        {
            get { return _membershipNumber; }
            set { if (Set("MembershipNumber", ref _membershipNumber, value)) Dirty = true; }
        }

        public DateTime? MembershipStartDate
        {
            get { return _membershipStartDate; }
            set
            {
                if (Set("MembershipStartDate", ref _membershipStartDate, value))
                {
                    Dirty = true;
                    MembershipExpiryDate = value.HasValue ? (DateTime?)value.Value.AddYears(1) : null;
                }
            }
        }

        public string Name
        {
            get { return _name; }
            set { if (Set("Name", ref _name, value)) Dirty = true; }
        }

        public string Phone
        {
            get { return _phone; }
            set { if (Set("Phone", ref _phone, value)) Dirty = true; }
        }

        [JsonIgnore]
        public bool Selected
        {
            get { return _selected; }
            set {Set("Selected", ref _selected, value); }
        }

        public int TotalGuests
        {
            get
            {
                return Games.Where(game => (DateTime.Now - game.GameDate).Days < 30).Sum(game => game.Guests);
            }
        }

        #endregion Properties

        #region Methods

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Person)obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public void SetPerson(Person person)
        {
            foreach (var prop in person.GetType().GetProperties().Where(prop => prop.CanWrite))
            {
                GetType().GetProperty(prop.Name).SetValue(this, prop.GetValue(person));
            }
        }

        protected bool Equals(Person other)
        {
            return Id == other.Id;
        }

        #endregion Methods
    }
}