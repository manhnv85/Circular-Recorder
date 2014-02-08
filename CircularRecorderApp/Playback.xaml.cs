using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace CircularRecorderApp
    {
    public partial class Playback : PhoneApplicationPage
        {

        public Playback ()
            {
            InitializeComponent ();
            var recordings = FileHandler.GetAllRecordings ();

            if (recordings.Any ())
                {
                _noRecodingErrorTextBlock.Visibility = Visibility.Collapsed;

                foreach (var recording in recordings)
                    {
                    if ((Visibility)Resources["PhoneLightThemeVisibility"] == Visibility.Visible)
                        {
                        recording.ImageIconUri = "Images/Play_Light.png";
                        }
                    else if ((Visibility)Resources["PhoneDarkThemeVisibility"] == Visibility.Visible)
                        {
                        recording.ImageIconUri = "Images/Play_Dark.png";
                        }
                    }

                _recordingListBox.DataContext = recordings;
                }
            else
                {
                _noRecodingErrorTextBlock.Visibility = Visibility.Visible;
                }
            }

        private void Play_Button_Click (object sender, GestureEventArgs e)
            {
            if (null != _recordingListBox.SelectedItem)
                {
                try
                    {
                    var mediaPlayerLauncher = new MediaPlayerLauncher ();
                    mediaPlayerLauncher.Media = new Uri ((_recordingListBox.SelectedItem as Recording).Name + ".wav", UriKind.Relative);
                    _recordingListBox.SelectedIndex = -1;
                    mediaPlayerLauncher.Location = MediaLocationType.Data;
                    mediaPlayerLauncher.Controls = MediaPlaybackControls.All;
                    mediaPlayerLauncher.Orientation = MediaPlayerOrientation.Landscape;
                    mediaPlayerLauncher.Show ();
                    }
                catch
                    {
                    }
                }
            }

        private void Delete_Item_Click (object sender, RoutedEventArgs e)
            {
            var recording = (_recordingListBox.ItemContainerGenerator.ContainerFromItem ((sender as MenuItem).DataContext) as ListBoxItem).DataContext as Recording;
            FileHandler.DeleteFile (recording.Name);
            (_recordingListBox.DataContext as ObservableCollection<Recording>).Remove (recording);
            }
        }
    }