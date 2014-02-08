using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace CircularRecorderApp
    {
    public partial class PlaySoundControl : UserControl
        {
        public PlaySoundControl ()
            {
            InitializeComponent ();

            if ((Visibility)Resources["PhoneLightThemeVisibility"] == Visibility.Visible)
                {
                _sound_0.Fill = new SolidColorBrush (Colors.Black);
                _sound_1.Fill = new SolidColorBrush (Colors.Black);
                _sound_2.Fill = new SolidColorBrush (Colors.Black);
                _sound_3.Fill = new SolidColorBrush (Colors.Black);
                }
            else if ((Visibility)Resources["PhoneDarkThemeVisibility"] == Visibility.Visible)
                {
                _sound_0.Fill = new SolidColorBrush (Colors.White);
                _sound_1.Fill = new SolidColorBrush (Colors.White);
                _sound_2.Fill = new SolidColorBrush (Colors.White);
                _sound_3.Fill = new SolidColorBrush (Colors.White);
                }
            PlaySoundStoryBoard.RepeatBehavior = RepeatBehavior.Forever;
            Visibility = Visibility.Collapsed;
            }

        public void Start ()
            {
            PlaySoundStoryBoard.Begin ();
            Visibility = Visibility.Visible;
            }

        public void Stop ()
            {
            PlaySoundStoryBoard.Stop ();
            Visibility = Visibility.Collapsed;
            }
        }
    }
