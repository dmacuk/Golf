using System;
using GalaSoft.MvvmLight;

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
}