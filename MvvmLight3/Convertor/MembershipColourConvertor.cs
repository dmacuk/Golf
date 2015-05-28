using GolfClub.Model;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GolfClub.Convertor
{
    public enum AlertState
    {
        None,
        Active,
        DueToExpire,
        Expired
    }

    public static class MembershipAlert
    {
        #region Methods

        public static AlertState Alert(this DateTime? date, int alertDays)
        {
            if (date == null) return AlertState.None;
            if (date < DateTime.Now) return AlertState.Expired;
            if (date < DateTime.Now.AddDays(alertDays)) return AlertState.DueToExpire;
            return AlertState.Active;
        }

        #endregion Methods
    }

    public class MembershipColourConvertor : IValueConverter
    {
        #region Fields

        private const int AlertDays = 30;
        private readonly SolidColorBrush _alertColour = Brushes.Yellow;
        private readonly SolidColorBrush _defaultColour = Brushes.White;
        private readonly SolidColorBrush _overDueColour = Brushes.Red;

        #endregion Fields

        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var person = value as Person;
            Debug.Assert(person != null);
            var date = person.MembershipExpiryDate;
            return GetRowColour(date);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private SolidColorBrush GetRowColour(DateTime? date)
        {
            var alertState = date.Alert(AlertDays);
            switch (alertState)
            {
                case AlertState.None:
                case AlertState.Active:
                    return _defaultColour;

                case AlertState.DueToExpire:
                    return _alertColour;

                case AlertState.Expired:
                    return _overDueColour;

                default:
                    throw new InvalidEnumArgumentException(@"Illegal enum AlertState: " + alertState);
            }
        }

        #endregion Methods
    }
}