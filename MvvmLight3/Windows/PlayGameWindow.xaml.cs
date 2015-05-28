using GolfClub.Model;
using GolfClub.Utilities;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace GolfClub.Windows
{
    /// <summary>
    /// Interaction logic for PlayGameWindow.xaml
    /// </summary>
    public partial class PlayGameWindow
    {
        #region Fields

        private readonly Person _person;

        #endregion Fields

        #region Constructors

        public PlayGameWindow(Person person)
        {
            InitializeComponent();
            this.LoadWindowSettings();
            DataContext = person;
            GameDate.Value = DateTime.Now;
            TextNumberGuests.Text = "0";
            _person = person;
        }

        #endregion Constructors

        #region Methods

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            if (GameDate.Value != null)
                _person.Games.Add(new Game { GameDate = GameDate.Value.Value, Guests = int.Parse(TextNumberGuests.Text) });
            Close();
        }

        private void ValidateNumber(object sender, TextCompositionEventArgs e)
        {
            try
            {
                // ReSharper disable ReturnValueOfPureMethodIsNotUsed
                int.Parse(e.Text);
                // ReSharper restore ReturnValueOfPureMethodIsNotUsed
                e.Handled = false;
            }
            catch
            {
                e.Handled = true;
            }
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            this.SaveWindowSettings();
        }

        #endregion Methods
    }
}