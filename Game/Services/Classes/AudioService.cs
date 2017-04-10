using System;
using System.Collections.Generic;
using System.Linq;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using OpenGL_Practice.Services.Interfaces;

namespace OpenGL_Practice.Services.Classes
{
    public class AudioService : IDisposable, IAudioService
    {
        public static readonly AudioService Instance = new AudioService();
        private readonly MixingSampleProvider _mixer;
        private readonly IWavePlayer _outputDevice;

        public AudioService(int sampleRate = 44100, int channelCount = 2)
        {
            _outputDevice = new WaveOutEvent();
            _mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount))
            {
                ReadFully = true
            };
            _outputDevice.Init(_mixer);
            _outputDevice.Play();
        }

        public void Dispose()
        {
            _outputDevice.Dispose();
        }

        public void PlaySound(string fileName, bool loop = false)
        {
            var input = new AudioFileReader(Assets.GetSound(fileName));
            AddMixerInput(new AutoDisposeFileReader(input));
        }

        private ISampleProvider ConvertToRightChannelCount(ISampleProvider input)
        {
            if (input.WaveFormat.Channels == _mixer.WaveFormat.Channels)
                return input;
            if (input.WaveFormat.Channels == 1 && _mixer.WaveFormat.Channels == 2)
                return new MonoToStereoSampleProvider(input);
            throw new NotImplementedException("Not yet implemented this channel count conversion");
        }

        private void AddMixerInput(ISampleProvider input)
        {
            _mixer.AddMixerInput(ConvertToRightChannelCount(input));
        }
    }

    internal class AutoDisposeFileReader : ISampleProvider
    {
        private readonly AudioFileReader _reader;
        private bool _isDisposed;
        private bool _looping;

        public AutoDisposeFileReader(AudioFileReader reader)
        {
            _reader = reader;
            WaveFormat = reader.WaveFormat;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            if (_isDisposed)
                return 0;
            var read = _reader.Read(buffer, offset, count);

            if (read != 0) return read;

            _reader.Dispose();
            _isDisposed = true;
            return read;
        }

        public WaveFormat WaveFormat { get; }
    }

    internal class CachedSoundSampleProvider : ISampleProvider
    {
        private readonly CachedSound _cachedSound;
        private long _position;

        public CachedSoundSampleProvider(CachedSound cachedSound)
        {
            _cachedSound = cachedSound;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            var availableSamples = _cachedSound.AudioData.Length - _position;
            var samplesToCopy = Math.Min(availableSamples, count);
            Array.Copy(_cachedSound.AudioData, _position, buffer, offset, samplesToCopy);
            _position += samplesToCopy;
            return (int) samplesToCopy;
        }

        public WaveFormat WaveFormat => _cachedSound.WaveFormat;
    }

    internal class CachedSound
    {
        public CachedSound(string audioFileName)
        {
            using (var audioFileReader = new AudioFileReader(audioFileName))
            {
                WaveFormat = audioFileReader.WaveFormat;
                var wholeFile = new List<float>((int) (audioFileReader.Length / 4));
                var readBuffer = new float[audioFileReader.WaveFormat.SampleRate * audioFileReader.WaveFormat.Channels];
                int samplesRead;
                while ((samplesRead = audioFileReader.Read(readBuffer, 0, readBuffer.Length)) > 0)
                    wholeFile.AddRange(readBuffer.Take(samplesRead));
                AudioData = wholeFile.ToArray();
            }
        }

        public float[] AudioData { get; }
        public WaveFormat WaveFormat { get; }
    }
}