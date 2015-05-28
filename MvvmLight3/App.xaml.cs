using GalaSoft.MvvmLight.Threading;
using System.Windows;

namespace GolfClub
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Constructors

        static App()
        {
            DispatcherHelper.Initialize();
        }

        #endregion Constructors
    }
}