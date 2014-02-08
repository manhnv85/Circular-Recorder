using System;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Media.Imaging;
using CircularRecorder;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework.Audio;


namespace CircularRecorderApp
    {
    public partial class MainPage : PhoneApplicationPage
        {
        private ProgressIndicator _progressIndicatror;
        private Recorder _recorder;
        private bool _isRecording;
        private bool _isPlaying;
        private bool _isRecordingPaused;

        private ApplicationBarIconButton _pauseButton;
        private ApplicationBarIconButton _recordButton;
        private ApplicationBarIconButton _stopButton;
        private ApplicationBarIconButton _playButton;
        private ApplicationBarMenuItem _saveMenuItem;


        private bool _wasRecordingBeforeLock;

        // Constructor
        public MainPage ()
            {
            InitializeComponent ();
            Utilities.MainPageControl = this;
            Unloaded += Page_Unloaded;
            InitializeAppBarButtons ();
            _saveMenuItem = ApplicationBar.MenuItems[0] as ApplicationBarMenuItem;
            _saveMenuItem.IsEnabled = false;

            if (!IsolatedStorageSettings.ApplicationSettings.Contains (Constants.TOTAL_SECONDS))
                IsolatedStorageSettings.ApplicationSettings.Add (Constants.TOTAL_SECONDS, "60");

            if (!IsolatedStorageSettings.ApplicationSettings.Contains (Constants.MINUTE_SECOND_KEY))
                IsolatedStorageSettings.ApplicationSettings.Add (Constants.MINUTE_SECOND_KEY, Constants.MINUTE);

            IsolatedStorageSettings.ApplicationSettings.Save ();

            UpdateButtonState (false, false, true, false, false);

            _progressIndicatror = new ProgressIndicator
            {
                IsVisible = false,
                IsIndeterminate = true,
                Text = "Stopping..."
            };
            SystemTray.SetProgressIndicator (this, _progressIndicatror);

            }

        private void Page_Unloaded (object sender, RoutedEventArgs e)
            {
            if (_isPlaying)
                {
                _recorder.Stop_Playing_Sound ();
                _soundPlayingAnimation.Stop ();
                }

            if (_isRecording || _isRecordingPaused)
                Stop ();

            UpdateButtonState (true, false, true, false, false);
            Utilities.MainPageControl = null;

            foreach (var lifeTimeObject in App.Current.ApplicationLifetimeObjects)
                {
                if (lifeTimeObject is XNAFrameworkDispatcherService)
                    (lifeTimeObject as XNAFrameworkDispatcherService).frameworkDispatcherTimer.Tick -= XNA_Timer_Tick;
                }

            _progressIndicatror.IsVisible = false;
            }

        private void InitializeAppBarButtons ()
            {
            if ((Visibility)Resources["PhoneLightThemeVisibility"] == Visibility.Visible)
                {
                _pauseButton = new ApplicationBarIconButton (new Uri ("Images/Pause_Light.png", UriKind.RelativeOrAbsolute));
                _stopButton = new ApplicationBarIconButton (new Uri ("Images/Stop_Light.png", UriKind.RelativeOrAbsolute));
                _playButton = new ApplicationBarIconButton (new Uri ("Images/Play_Light.png", UriKind.RelativeOrAbsolute));
                _recordButton = new ApplicationBarIconButton (new Uri ("Images/Record_Light.png", UriKind.RelativeOrAbsolute));
                _pressToPlayImage.Source = new BitmapImage (new Uri ("Images/Press_To_Record_Light.png", UriKind.RelativeOrAbsolute));
                }
            else if ((Visibility)Resources["PhoneDarkThemeVisibility"] == Visibility.Visible)
                {
                _pauseButton = new ApplicationBarIconButton (new Uri ("Images/Pause_Dark.png", UriKind.RelativeOrAbsolute));
                _stopButton = new ApplicationBarIconButton (new Uri ("Images/Stop_Dark.png", UriKind.RelativeOrAbsolute));
                _playButton = new ApplicationBarIconButton (new Uri ("Images/Play_Dark.png", UriKind.RelativeOrAbsolute));
                _recordButton = new ApplicationBarIconButton (new Uri ("Images/Record_Dark.png", UriKind.RelativeOrAbsolute));
                _pressToPlayImage.Source = new BitmapImage (new Uri ("Images/Press_To_Record_Dark.png", UriKind.RelativeOrAbsolute));
                }

            _pauseButton.Click += Pause_Button_Click;
            _playButton.Click += Play_Button_Click;
            _recordButton.Click += Record_Button_Click;
            _stopButton.Click += Stop_Button_Click;

            _pauseButton.Text = "pause";
            _playButton.Text = "play";
            _recordButton.Text = "record";
            _stopButton.Text = "stop";

            ApplicationBar.Buttons.Add (_recordButton);
            ApplicationBar.Buttons.Add (_stopButton);
            ApplicationBar.Buttons.Add (_pauseButton);
            ApplicationBar.Buttons.Add (_playButton);

            foreach (var lifeTimeObject in App.Current.ApplicationLifetimeObjects)
                {
                if (lifeTimeObject is XNAFrameworkDispatcherService)
                    (lifeTimeObject as XNAFrameworkDispatcherService).frameworkDispatcherTimer.Tick += XNA_Timer_Tick;
                }
            }

        private void Play ()
            {
            try
                {
                UpdateButtonState (false, true, true, true, true);
                _isPlaying = true;
                if (null != _recorder)
                    {
                    _recorder.Play ();
                    _soundPlayingAnimation.Start ();
                    }
                }
            catch (Exception ex)
                {
                }
            }

        private void Record ()
            {
            _pressToPlayImage.Visibility = Visibility.Collapsed;

            _isRecording = true;

            if (_isPlaying)
                {
                _recorder.Stop_Playing_Sound ();
                _soundPlayingAnimation.Stop ();
                }

            if (!_isRecordingPaused)
                {
                var totalSeconds = int.Parse (IsolatedStorageSettings.ApplicationSettings[Constants.TOTAL_SECONDS].ToString ());

                if (null == _recorder || _recorder.TotalSeconds != totalSeconds)
                    {
                    _recorder = new Recorder (totalSeconds);
                    _animationControl.TimeInSeconds = Convert.ToDouble (totalSeconds);
                    }
                _recorder.StartRecording ();

                //start
                _animationControl.Play ();

                }
            else
                {
                _recorder.Resume ();
                //Resume
                _animationControl.Resume ();
                }

            _isRecordingPaused = false;
            UpdateButtonState (false, true, false, true, false);
            }

        private void Pause ()
            {
            if (_isRecording)
                {
                _isRecordingPaused = true;
                _recorder.Pause ();
                UpdateButtonState (false, false, true, true, false);
                //pause
                _animationControl.Pause ();
                }
            else
                {
                _soundPlayingAnimation.Stop ();
                _soundPlayingAnimation.Stop ();
                UpdateButtonState (true, false, true, true, true);
                }
            }

        private void Stop ()
            {
            _saveMenuItem.IsEnabled = true;

            if (_isRecording)
                {
                _progressIndicatror.IsVisible = true;
                _isRecording = false;
                _isRecordingPaused = false;
                //stop
                _animationControl.Stop ();

                _recorder.StopRecording ();
                _progressIndicatror.IsVisible = false;
                }
            else
                {
                _recorder.Stop_Playing_Sound ();
                _soundPlayingAnimation.Stop ();
                }

            UpdateButtonState (true, false, true, false, true);
            }

        private void Play_Button_Click (object sender, EventArgs e)
            {
            Play ();
            }

        private void Stop_Button_Click (object sender, EventArgs e)
            {
            Stop ();
            }

        private void Pause_Button_Click (object sender, EventArgs e)
            {
            Pause ();
            }

        private void Record_Button_Click (object sender, EventArgs e)
            {
            Record ();
            }

        private void Save_Button_Click (object sender, EventArgs e)
            {
            Utilities.MemoryStream = _recorder.SavedStream;
            NavigationService.Navigate (new Uri ("/SaveRecording.xaml", UriKind.RelativeOrAbsolute));
            }

        private void Settings_Button_Click (object sender, EventArgs e)
            {
            NavigationService.Navigate (new Uri ("/Settings.xaml", UriKind.RelativeOrAbsolute));
            }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Play"></param>
        /// <param name="Pause"></param>
        /// <param name="Record"></param>
        /// <param name="Stop"></param>
        private void UpdateButtonState (bool play, bool pause, bool record, bool stop, bool save)
            {
            _playButton.IsEnabled = play;
            _pauseButton.IsEnabled = pause;
            _recordButton.IsEnabled = record;
            _stopButton.IsEnabled = stop;
            _saveMenuItem.IsEnabled = save;
            }


        /// <summary>
        /// Updates the XNA FrameworkDispatcher and checks to see if a sound is playing.
        /// If sound has stopped playing, it updates the UI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void XNA_Timer_Tick (object sender, EventArgs e)
            {
            if (_isPlaying)
                if (null != _recorder)
                    {
                        {
                        if (null != _recorder.SoundEffect)
                            {
                            if (!_recorder.SoundEffect.IsDisposed && _recorder.SoundEffect.State == SoundState.Stopped)
                                {
                                _isPlaying = false;
                                UpdateButtonState (true, false, true, false, true);
                                _soundPlayingAnimation.Stop ();
                                }
                            }
                        }
                    }
            }

        public void OnScreenLock ()
            {
            if (_isRecording && !_isRecordingPaused)
                {
                _wasRecordingBeforeLock = true;
                _animationControl.OnScreenLock ();
                }
            else
                {
                _wasRecordingBeforeLock = false;
                }
            }

        public void OnScreenUnlock ()
            {
            if (_wasRecordingBeforeLock)
                _animationControl.OnScreenUnlock ();

            _wasRecordingBeforeLock = false;
            }

        }
    }