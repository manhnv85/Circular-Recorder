using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Shell;

namespace CircularRecorderApp
    {
    public partial class Settings : PhoneApplicationPage
        {
        private ApplicationBarIconButton _saveButton;

        public Settings ()
            {
            InitializeComponent ();

            if ((Visibility)Resources["PhoneLightThemeVisibility"] == Visibility.Visible)
                {
                _saveButton = new ApplicationBarIconButton (new Uri ("Images/Save_Light.png", UriKind.RelativeOrAbsolute));
                }
            else if ((Visibility)Resources["PhoneDarkThemeVisibility"] == Visibility.Visible)
                {
                _saveButton = new ApplicationBarIconButton (new Uri ("Images/Save_Dark.png", UriKind.RelativeOrAbsolute));
                }

            _saveButton.Click += Save_Button_Click;
            _saveButton.Text = "save";
            ApplicationBar.Buttons.Add (_saveButton);


            _secondMinutePicker.ItemsSource = new List<string> () { Constants.SECOND, Constants.MINUTE };

            var selectedOption = IsolatedStorageSettings.ApplicationSettings[Constants.MINUTE_SECOND_KEY].ToString ();
            _secondMinutePicker.SelectedItem = selectedOption;

            int bufferLength = int.Parse (IsolatedStorageSettings.ApplicationSettings[Constants.TOTAL_SECONDS].ToString ());

            if (selectedOption == Constants.MINUTE)
                bufferLength = bufferLength / 60;

            _timeTextBox.Text = bufferLength.ToString ();
            }

        private void Save_Button_Click (object sender, EventArgs e)
            {
            _errorTextBlock.Visibility = Visibility.Collapsed;

            if (string.IsNullOrWhiteSpace (_errorTextBlock.Text))
                {
                _errorTextBlock.Visibility = Visibility.Visible;
                return;
                }

            var length = 0;
            try
                {
                length = int.Parse (_timeTextBox.Text);
                }
            catch
                {
                _errorTextBlock.Visibility = Visibility.Visible;
                return;
                }

            if (_secondMinutePicker.SelectedItem.ToString () == Constants.MINUTE)
                length = length * 60;

            if (length < 5 || length > 60 * 60)
                {
                _errorTextBlock.Visibility = Visibility.Visible;
                return;
                }

            if (length > 100)
                {
                MessageBox.Show ("Free version can only save recording upto 100 seconds", "Error", MessageBoxButton.OK);
                return;
                }

            IsolatedStorageSettings.ApplicationSettings[Constants.TOTAL_SECONDS] = length.ToString ();
            IsolatedStorageSettings.ApplicationSettings[Constants.MINUTE_SECOND_KEY] = _secondMinutePicker.SelectedItem.ToString ();
            IsolatedStorageSettings.ApplicationSettings.Save ();
            NavigationService.GoBack ();
            }

        private void KeyUp (object sender, KeyEventArgs e)
            {
            TextBox txt = (TextBox)sender;
            if (txt.Text.Contains ("."))
                {
                txt.Text = txt.Text.Replace (".", "");
                txt.SelectionStart = txt.Text.Length;
                }
            }
        }
    }