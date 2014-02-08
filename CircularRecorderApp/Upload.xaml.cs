using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Live;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace CircularRecorderApp
    {
    public partial class Upload : PhoneApplicationPage
        {
        private ProgressIndicator _progressIndicatror;
        private bool _isSignout;

        public Upload ()
            {
            InitializeComponent ();
            _isSignout = false;
            var recordings = FileHandler.GetAllRecordings ();
            if (recordings.Any ())
                {
                _noRecodingErrorTextBlock.Visibility = Visibility.Collapsed;

                foreach (var recording in recordings)
                    {
                    if ((Visibility)Resources["PhoneLightThemeVisibility"] == Visibility.Visible)
                        {
                        recording.ImageIconUri = "Images/Upload_Light.png";
                        }
                    else if ((Visibility)Resources["PhoneDarkThemeVisibility"] == Visibility.Visible)
                        {
                        recording.ImageIconUri = "Images/Upload_Dark.png";
                        }
                    }

                _recordingListBox.DataContext = recordings;
                }
            else
                {
                _noRecodingErrorTextBlock.Visibility = Visibility.Visible;
                }
            }

        protected override void OnNavigatingFrom (NavigatingCancelEventArgs e)
            {
            if (e.NavigationMode == NavigationMode.Back)
                if (!_isSignout)
                    NavigationService.RemoveBackEntry ();

            base.OnNavigatingFrom (e);
            }

        private void Upload_Button_Click (object sender, GestureEventArgs e)
            {
            if (null == _progressIndicatror || !_progressIndicatror.IsVisible)
                {
                if (null != _recordingListBox.SelectedItem)
                    {
                    var recording = _recordingListBox.SelectedItem as Recording;
                    if (null == Utilities.SkyDriveSession)
                        NavigationService.GoBack ();
                    else
                        UploadFile (recording.Name);
                    }
                }
            }

        private void UploadFile (string name)
            {
            try
                {
                using (var storage = IsolatedStorageFile.GetUserStoreForApplication ())
                    {
                    var fileStream = storage.OpenFile (name + ".wav", FileMode.Open);
                    var uploadClient = new LiveConnectClient (Utilities.SkyDriveSession);
                    uploadClient.UploadCompleted += UploadClient_UploadCompleted;
                    uploadClient.UploadAsync ("me/skydrive", name + ".wav", fileStream, OverwriteOption.Rename);
                    _progressIndicatror = new ProgressIndicator
                    {
                        IsVisible = true,
                        IsIndeterminate = true,
                        Text = "Uploading file..."
                    };

                    SystemTray.SetProgressIndicator (this, _progressIndicatror);
                    }
                }
            catch (Exception)
                {
                ShowError ();
                NavigationService.GoBack ();
                }
            }

        private void UploadClient_UploadCompleted (object sender, LiveOperationCompletedEventArgs e)
            {
            _progressIndicatror.IsVisible = false;

            if (null == e.Error)
                MessageBox.Show ("Successfully uploaded the file to SkyDrive", "File Uploaded", MessageBoxButton.OK);
            else
                ShowError ();
            }

        private void ShowError ()
            {

            MessageBox.Show ("Unexpected error occured, please try later", "Upload Failed", MessageBoxButton.OK);
            }

        private void Sign_Out_Button_Click (object sender, EventArgs e)
            {
            _isSignout = true;
            NavigationService.GoBack ();
            }
        }
    }