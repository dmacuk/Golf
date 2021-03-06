﻿using System.ComponentModel;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GolfClub.Convertor;
using GolfClub.Design;
using GolfClub.Interfaces;
using GolfClub.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Mail;
using System.Windows.Input;
using GolfClub.Properties;

namespace GolfClub.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields

        private readonly IDataService _dataService;
        private readonly IWindowService _windowService;
        private readonly IFileService _fileService;

        private int _dummy;
        private bool _listDirty;
        private ObservableCollection<Person> _people;
        private string _search;
        private Visibility _showToolbarIcons;
        private Visibility _showToolbarText;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService, IWindowService windowService, IFileService fileService)
        {
            _dataService = dataService;
            _windowService = windowService;
            _fileService = fileService;
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
            ToolbarSettings();
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

        public Visibility ShowToolbarIcons
        {
            get { return _showToolbarIcons; }
            set { Set("ShowToolbarIcons", ref _showToolbarIcons, value); }
        }

        public Visibility ShowToolbarText
        {
            get { return _showToolbarText; }
            set { Set("ShowToolbarText", ref _showToolbarText, value); }
        }

        public ICommand SettingsCommand { get; set; }

        public ICommand ReportCommand { get; set; }

        public ICommand SelectDueCommand { get; set; }

        public ICommand SelectExpiredCommand { get; set; }

        public ICommand ExportCommand { get; set; }

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
                if (emailAddressList.Count > 0)
                {
                    _windowService.LaunchEmailWindow(emailAddressList);
                }
            });

            SelectAllCommand = new RelayCommand(() =>
            {
                foreach (var person in People)
                {
                    person.Selected = true;
                }
            });

            SelectNoneCommand = new RelayCommand(SelectNone);

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
                UpdateList();
            }, person => person != null);

            PlayGolfCommand = new RelayCommand<Person>(person =>
            {
                _windowService.PlayGame(person);
                UpdateList();
            }, person => person != null);

            SettingsCommand = new RelayCommand(() =>
            {
                _windowService.LaunchSettingsWindow();
                ToolbarSettings();
            });

            ReportCommand = new RelayCommand<string>(selection => _windowService.Report(GetReportTitle(selection), GetReportData(selection)));

            ExportCommand = new RelayCommand<string>(selection => _fileService.WriteCvFile(GetReportData(selection)));

            SelectDueCommand = new RelayCommand(() => Select(MembershipDuePeople()));

            SelectExpiredCommand = new RelayCommand(() => Select(MembershipExpiredPeople()));

        }

        private static string GetReportTitle(string selection)
        {
            switch (selection)
            {
                case "All":
                    return "All Members";
                case "Expired":
                    return "Expired Members";
                case "Due":
                    return "Due Members";
                case "Selected":
                    return "Selected Members";
                default:
                    throw new InvalidEnumArgumentException(@"Invalid selection: " + selection);
            }
        }

        private List<Person> GetReportData(string selection)
        {
            IEnumerable<Person> data;
            switch (selection)
            {
                case "All":
                    data = People;
                    break;
                case "Expired":
                    data = MembershipExpiredPeople();
                    break;
                case "Due":
                    data = MembershipDuePeople();
                    break;
                case "Selected":
                    data = People.Where(p => p.Selected);
                    break;
                default:
                    throw new InvalidEnumArgumentException(@"Invalid selection: "+selection);
            }
            return data.ToList();
        }

        private void Select(IEnumerable<Person> selection)
        {
            SelectNone();
            foreach (var person in selection)
            {
                person.Selected = true;
            }
        }

        private void SelectNone()
        {
            foreach (var person in People)
            {
                person.Selected = false;
            }
        }

        private IEnumerable<Person> MembershipDuePeople()
        {
            return People.Where(p => p.MembershipExpiryDate.Alert(MembershipColourConvertor.AlertDays) == AlertState.DueToExpire);
        }

        private IEnumerable<Person> MembershipExpiredPeople()
        {
            return People.Where(p=>p.MembershipExpiryDate.Alert(MembershipColourConvertor.AlertDays)==AlertState.Expired);
        }

        private List<string> GetEmailAddressList()
        {
            return (from person in People where person.Selected where IsValidEmailAddress(person.Email) select person.Email).ToList();
        }

        private void UpdateList(object sender = null, NotifyCollectionChangedEventArgs e = null)
        {
            _listDirty = true;
            Set("FilteredPeople", ref _dummy, _dummy == 0 ? 1 : 0);
        }

        private void ToolbarSettings()
        {
            ShowToolbarIcons = GetVisiblity(Settings.Default.ShowToolbarIcons);
            ShowToolbarText = GetVisiblity(Settings.Default.ShowToolbarText);
        }

        private static Visibility GetVisiblity(bool state)
        {
            return state ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion Methods
    }
}