using System;
using System.Windows.Navigation;
using Microsoft.Live;
using Microsoft.Live.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace CircularRecorderApp
    {
    public partial class SignIn : PhoneApplicationPage
        {
        private ProgressIndicator _progressIndicator;

        public SignIn ()
            {
            InitializeComponent ();
            _progressIndicator = new ProgressIndicator ();
            _signInButton.Scopes = "wl.signin wl.skydrive_update";

            _progressIndicator = new ProgressIndicator
            {
                IsVisible = true,
                IsIndeterminate = true,
                Text = "Connecting..."
            };

            SystemTray.SetProgressIndicator (this, _progressIndicator);
            }

        private void SignIn_SessionChanged (object sender, LiveConnectSessionChangedEventArgs e)
            {
            _progressIndicator.IsVisible = false;

            if (e.Status == LiveConnectSessionStatus.Connected)
                {
                Utilities.SkyDriveSession = e.Session;
                NavigationService.Navigate (new Uri ("/Upload.xaml", UriKind.RelativeOrAbsolute));
                }

            }

        private void Sign_In_Button_Tap (object sender, GestureEventArgs e)
            {
            }
        }
    }
