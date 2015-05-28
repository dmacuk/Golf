using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GolfClub.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Mail;
using System.Windows.Input;

namespace GolfClub.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields

        private readonly IDataService _dataService;
        private readonly IWindowService _windowService;

        private int _dummy;
        private bool _listDirty;
        private ObservableCollection<Person> _people;
        private string _search;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService, IWindowService windowService)
        {
            if (dataService == null) throw new ArgumentNullException("dataService");
            _dataService = dataService;
            _windowService = windowService;
            _dataService.Load((people, error) =>
            {
                if (error != null)
                {
                    return;
                }
                People = new ObservableCollection<Person>(people);
                People.CollectionChanged += UpdateList;
            });

            CreateCommands();
        }

        #endregion Constructors

        #region Properties

        public ICommand AddCommand { get; private set; }

        public ICommand DeleteCommand { get; private set; }

        public ICommand EditCommand { get; set; }

        public ICommand EmailCommand { get; set; }

        public ObservableCollection<Person> FilteredPeople
        {
            get { return new ObservableCollection<Person>(string.IsNullOrWhiteSpace(Search) ? People : People.Where(person => string.IsNullOrWhiteSpace(person.Name) || person.Name.ToLower().Contains(Search))); }
        }

        public ICommand InvertSelectionCommand { get; set; }

        public ObservableCollection<Person> People
        {
            get { return _people; }
            set
            {
                Set("People", ref _people, value);
            }
        }

        public ICommand PlayGolfCommand { get; set; }

        public ICommand RenewMemberShipCommand { get; set; }

        public ICommand SaveCommand { get; set; }

        public string Search
        {
            get { return _search; }
            set
            {
                Set("FilteredPeople", ref _search, value);
            }
        }

        public ICommand SelectAllCommand { get; set; }

        public ICommand SelectNoneCommand { get; set; }

        #endregion Properties

        #region Methods

        public override void Cleanup()
        {
            // Clean up if needed
            if (People.Any(person => person.Dirty) || _listDirty) Save();
            base.Cleanup();
        }

        public void Save()
        {
            _dataService.Save(new List<Person>(People));
        }

        private static bool IsValidEmailAddress(string emailaddress)
        {
            try
            {
                // ReSharper disable ObjectCreationAsStatement
                new MailAddress(emailaddress);
                // ReSharper restore ObjectCreationAsStatement

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private void CreateCommands()
        {
            AddCommand = new RelayCommand(() =>
            {
                var person = new Person();
                if (_windowService.EditPerson(person))
                {
                    var id = People.Max(p => p.Id);
                    person.Id = id;
                    People.Add(person);
                }
            });

            DeleteCommand = new RelayCommand<Person>(person => People.Remove(person), person => person != null);

            EditCommand = new RelayCommand<Person>(person =>
            {
                var original = new Person(person);
                if (!_windowService.EditPerson(person))
                {
                    person.SetPerson(original);
                }
            }, person => person != null);

            SaveCommand = new RelayCommand(Save);

            EmailCommand = new RelayCommand(() =>
            {
                var emailAddressList = GetEmailAddressList();
                _windowService.LaunchEmailWindow(emailAddressList);
            });

            SelectAllCommand = new RelayCommand(() =>
            {
                foreach (var person in People)
                {
                    person.Selected = true;
                }
            });

            SelectNoneCommand = new RelayCommand(() =>
            {
                foreach (var person in People)
                {
                    person.Selected = false;
                }
            });

            InvertSelectionCommand = new RelayCommand(() =>
            {
                foreach (var person in People)
                {
                    person.Selected = !person.Selected;
                }
            });

            RenewMemberShipCommand = new RelayCommand<Person>(person =>
            {
                var original = new Person(person);
                if (_windowService.RenewMembership(person) != true)
                {
                    person.SetPerson(original);
                }
            }, person => person != null);

            PlayGolfCommand = new RelayCommand<Person>(person => _windowService.PlayGame(person), person => person != null);
        }

        private List<string> GetEmailAddressList()
        {
            return (from person in People where person.Selected && IsValidEmailAddress(person.Email) select person.Email).ToList();
        }

        private void UpdateList(object sender, NotifyCollectionChangedEventArgs e)
        {
            _listDirty = true;
            Set("FilteredPeople", ref _dummy, _dummy == 0 ? 1 : 0);
        }

        #endregion Methods
    }
}