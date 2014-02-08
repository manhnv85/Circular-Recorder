using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace CircularRecorderApp
    {
    public partial class SaveRecording : PhoneApplicationPage
        {
        private ApplicationBarIconButton _saveButton;

        public SaveRecording ()
            {
            InitializeComponent ();
            Unloaded += Save_Recording_Unloaded;

            if ((Visibility)Resources["PhoneLightThemeVisibility"] == Visibility.Visible)
                {
                _saveButton = new ApplicationBarIconButton (new Uri ("Images/Save_Light.png", UriKind.RelativeOrAbsolute));
                }
            else if ((Visibility)Resources["PhoneDarkThemeVisibility"] == Visibility.Visible)
                {
                _saveButton = new ApplicationBarIconButton (new Uri ("Images/Save_Dark.png", UriKind.RelativeOrAbsolute));
                }

            _saveButton.Text = "Save";
            _saveButton.Click += Save_Button_Click;
            ApplicationBar.Buttons.Add (_saveButton);
            }

        private void Save_Recording_Unloaded (object sender, RoutedEventArgs e)
            {
            Utilities.MemoryStream = null;
            }


        private void Save_Button_Click (object sender, EventArgs e)
            {
            _fileNameTextBox.Text = _fileNameTextBox.Text.Trim ();

            _errorTextBlock.Visibility = Visibility.Collapsed;

            if (string.IsNullOrWhiteSpace (_fileNameTextBox.Text))
                return;

            foreach (var charInName in _fileNameTextBox.Text)
                {
                if (!Char.IsLetterOrDigit (charInName) && charInName != ' ')
                    {
                    _errorTextBlock.Visibility = Visibility.Visible;
                    return;
                    }
                }

            if (FileHandler.DoesFileExists (_fileNameTextBox.Text))
                {
                var result = MessageBox.Show (string.Format ("File with name {0} already exists. Do you want to overwrite it?", _fileNameTextBox.Text), "Save", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.Cancel)
                    return;

                FileHandler.SaveFile (_fileNameTextBox.Text);
                }
            else
                {
                FileHandler.SaveFile (_fileNameTextBox.Text);
                }

            NavigationService.GoBack ();
            }
        }
    }