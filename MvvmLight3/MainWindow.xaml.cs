﻿using GolfClub.Utilities;
using GolfClub.ViewModel;
using System.ComponentModel;

namespace GolfClub
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.LoadWindowSettings();
            Closing += (s, e) =>
            {
                ViewModelLocator.Cleanup();
                ((MainViewModel)DataContext).Cleanup();
            };
        }

        #endregion Constructors

        #region Methods

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            this.SaveWindowSettings();
        }

        #endregion Methods
    }
}