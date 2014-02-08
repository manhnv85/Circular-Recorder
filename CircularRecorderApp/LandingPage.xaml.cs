using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace CircularRecorderApp
    {
    public partial class LandingPage : PhoneApplicationPage
        {
        public LandingPage ()
            {
            InitializeComponent ();

            if ((Visibility)Resources["PhoneLightThemeVisibility"] == Visibility.Visible)
                {
                _playImgae.Source = new BitmapImage (new Uri ("Images/Play_Light.png", UriKind.RelativeOrAbsolute));
                _recordImgae.Source = new BitmapImage (new Uri ("Images/Mic_Dark.png", UriKind.RelativeOrAbsolute));
                _uploadImgae.Source = new BitmapImage (new Uri ("Images/SkyDrive_Light.png", UriKind.RelativeOrAbsolute));
                }
            else if ((Visibility)Resources["PhoneDarkThemeVisibility"] == Visibility.Visible)
                {
                _playImgae.Source = new BitmapImage (new Uri ("Images/Play_Dark.png", UriKind.RelativeOrAbsolute));
                _recordImgae.Source = new BitmapImage (new Uri ("Images/Mic_Light.png", UriKind.RelativeOrAbsolute));
                _uploadImgae.Source = new BitmapImage (new Uri ("Images/SkyDrive_Dark.png", UriKind.RelativeOrAbsolute));
                }

            }

        private void Record_Button_Tapped (object sender, GestureEventArgs e)
            {
            NavigationService.Navigate (new Uri ("/MainPage.xaml", UriKind.RelativeOrAbsolute));
            }

        private void Play_Button_Tapped (object sender, GestureEventArgs e)
            {
            NavigationService.Navigate (new Uri ("/Playback.xaml", UriKind.RelativeOrAbsolute));
            }

        private void Upload_Button_Tapped (object sender, GestureEventArgs e)
            {
            NavigationService.Navigate (new Uri ("/SignIn.xaml", UriKind.RelativeOrAbsolute));
            }
        }
    }