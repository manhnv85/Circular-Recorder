using System;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace CircularRecorderApp
{
    public partial class AudioLoop : UserControl
    {
        private int _secondsPassed = 0;
        DispatcherTimer _animationTimer;

        public double TimeInSeconds
        {
            get;
            set;
        }

        public int SecondsPassed
        {
            get
            {
                return _secondsPassed;
            }
            set
            {
                _secondsPassed = value;
                TimeTextBlock.Text = _secondsPassed.ToString();
            }

        }

        public AudioLoop()
        {
            InitializeComponent();
        }

        public void OnScreenLock()
        {
            _animation.Pause();
        }

        public void OnScreenUnlock()
        {
            _animation.Seek(new TimeSpan(0, 0, 0, SecondsPassed));
            _animation.Resume();
        }

        public void Play()
        {
            SecondsPassed = 0;
            _animation.RepeatBehavior = RepeatBehavior.Forever;
            _animation.SpeedRatio = (1 / TimeInSeconds) * 300;
            _animation.Begin();
            // timer interval specified as 1 second
            _animationTimer = new DispatcherTimer();
            _animationTimer.Interval = TimeSpan.FromSeconds(1);
            // Sub-routine OnTimerTick will be called at every 1 second
            _animationTimer.Tick += OnTimerTick;
            // starting the timer
            _animationTimer.Start();
        }

        public void Pause()
        {
            _animation.Pause();
            _animationTimer.Stop();
        }

        public void Resume()
        {
            _animation.Resume();
            _animationTimer.Start();
        }

        public void Stop()
        {
            _animation.Stop();
            _animationTimer.Stop();
            SecondsPassed = 0;
        }

        void OnTimerTick(Object sender, EventArgs args)
        {
            SecondsPassed++;
        }

    }
}
