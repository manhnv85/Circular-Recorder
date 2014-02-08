using System;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework.Audio;

namespace CircularRecorder
    {
    public class Recorder
        {
        private Microphone _microPhone;     // Object representing the physical microphone on the device
        private byte[] _realTimeBuffer;                                  // Dynamic buffer to retrieve audio data from the microphone
        private MemoryStream _realTimeStream;       // Stores the audio data for later playback
        private MemoryStream _savedStream;
        private int _secondsPassed;
        private bool _isPaused;

        public Recorder (int totalRecordingLengthInSeconds)
            {
            _microPhone = Microphone.Default;
            TotalSeconds = totalRecordingLengthInSeconds;
            }

        public MemoryStream SavedStream
            {
            get
                {
                return _savedStream;
                }
            }

        public int TotalSeconds
            {
            get;
            set;
            }

        public SoundEffectInstance SoundEffect
            {
            get;
            set;
            }

        public void StartRecording ()
            {
            if (null != _savedStream)
                {
                _savedStream.Dispose ();
                }

            _secondsPassed = 0;

            // Get audio data in 1/2 second chunks
            _microPhone.BufferDuration = TimeSpan.FromMilliseconds (1000);

            // Allocate memory to hold the audio data
            _realTimeBuffer = new byte[_microPhone.GetSampleSizeInBytes (_microPhone.BufferDuration)];

            _realTimeStream = new MemoryStream ();

            AudioEncoder.WriteWavHeader (_realTimeStream, _microPhone.SampleRate);
            // Set the stream back to zero in case there is already something in it

            // Event handler for getting audio data when the buffer is full
            _microPhone.BufferReady += Microphone_Sound_Captured;

            // Start recording
            _microPhone.Start ();
            }

        public void StopRecording ()
            {
            _microPhone.BufferReady -= Microphone_Sound_Captured;
            if (_microPhone.State == MicrophoneState.Started)
                {
                // In RECORD mode, user clicked the 
                // stop button to end recording
                _microPhone.Stop ();
                AudioEncoder.UpdateWavHeader (_realTimeStream);
                }

            _savedStream = new MemoryStream (new byte[_realTimeStream.Length]);
            int startOfRecording = _secondsPassed < TotalSeconds ? 0 : _secondsPassed % TotalSeconds;
            _realTimeStream.Seek (startOfRecording * _microPhone.GetSampleSizeInBytes (_microPhone.BufferDuration), SeekOrigin.Begin);
            long current = _realTimeStream.Position;

            while (_realTimeStream.Position < _realTimeStream.Length)
                {
                _savedStream.WriteByte (byte.Parse (_realTimeStream.ReadByte ().ToString ()));
                }

            _realTimeStream.Seek (0, SeekOrigin.Begin);

            while (_realTimeStream.Position < current)
                _savedStream.WriteByte (byte.Parse (_realTimeStream.ReadByte ().ToString ()));

            _realTimeStream.Dispose ();
            _realTimeBuffer = null;
            }

        private void Microphone_Sound_Captured (object sender, EventArgs e)
            {
            // Retrieve audio data
            _microPhone.GetData (_realTimeBuffer);

            // Store the audio data in a stream
            _realTimeStream.Write (_realTimeBuffer, 0, _realTimeBuffer.Length);

            _secondsPassed++;

            if (_secondsPassed % TotalSeconds == 0)
                _realTimeStream.Seek (0, SeekOrigin.Begin);
            }

        public void Play ()
            {
            if (_savedStream.Length > 0)
                {
                // Update the UI to reflect that
                // sound is playing

                // Play the audio in a new thread so the UI can update.
                var soundThread = new Thread (Play_Sound);
                soundThread.Start ();
                }
            }

        private void Play_Sound ()
            {
            if (_isPaused)
                {
                _isPaused = false;
                SoundEffect.Resume ();
                }
            else
                {
                // Play audio using SoundEffectInstance so we can monitor it's State 
                // and update the UI in the dt_Tick handler when it is done playing.
                var sound = new SoundEffect (_savedStream.ToArray (), _microPhone.SampleRate, AudioChannels.Mono);
                SoundEffect = sound.CreateInstance ();
                SoundEffect.Play ();
                }
            }

        public void Stop_Playing_Sound ()
            {
            try
                {
                _isPaused = false;
                if (null != SoundEffect)
                    {
                    if (!SoundEffect.IsDisposed)
                        {
                        SoundEffect.Stop ();
                        SoundEffect.Dispose ();
                        }
                    }
                }
            catch (Exception ex)
                {
                }
            }

        public void Pause_Playing_Sound ()
            {
            _isPaused = true;
            SoundEffect.Pause ();
            }

        public void Pause ()
            {
            _microPhone.Stop ();
            _microPhone.BufferReady -= Microphone_Sound_Captured;
            }

        public void Resume ()
            {
            _microPhone.BufferReady += Microphone_Sound_Captured;
            _microPhone.Start ();
            }
        }
    }
