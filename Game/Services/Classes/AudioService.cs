using System;
using System.Collections.Generic;
using System.IO;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;

namespace OpenGL_Practice.Services.Classes
{
    internal class WaveReader
    {
        public string FileName { get; private set; }

        public string Signature => _signature;
        private string _signature;

        public int RiffSize { get; private set; }

        public string Format => _format;
        private string _format;

        public string FormatSig => _formatSig;
        private string _formatSig;

        public int FormatChunk { get; private set; }
        public int AudioFormat { get; private set; }
        public int Channels { get; private set; }
        public int SampleRate { get; private set; }
        public int ByteRate { get; private set; }
        public int Align { get; private set; }
        public int SampleBits { get; private set; }

        public string DataSig => _dataSig;
        private string _dataSig;

        public int DataChunk { get; private set; }
        public byte[] Data { get; private set; }

        public ALFormat SoundFormat =>
            Channels == 1
                ? SampleBits == 8
                    ? ALFormat.Mono8
                    : ALFormat.Mono16
                : SampleBits == 8
                    ? ALFormat.Stereo8
                    : ALFormat.Stereo16;

        public WaveReader(string fileName)
        {
            if (!fileName.Contains(".wav"))
                throw new NotSupportedException("ENGINE only supports RIFF format Wave files");

            var target = Assets.GetSound(fileName);
            if (!File.Exists(target)) throw new FileNotFoundException();

            using (var reader = new BinaryReader(File.Open(target, FileMode.Open)))
            {
                FileName = fileName;

                CheckDataString(out _signature, reader, 4, "RIFF");
                RiffSize = reader.ReadInt32();
                CheckDataString(out _format, reader, 4, "WAVE");
                CheckDataString(out _formatSig, reader, 4, "fmt ");
                FormatChunk = reader.ReadInt32();
                AudioFormat = reader.ReadInt16();
                Channels = reader.ReadInt16();
                SampleRate = reader.ReadInt32();
                ByteRate = reader.ReadInt32();
                Align = reader.ReadInt16();
                SampleBits = reader.ReadInt16();

                CheckDataString(out _dataSig, reader, 4, "data");

                DataChunk = reader.ReadInt32();

                Data = reader.ReadBytes((int)reader.BaseStream.Length);
            }
        }

        private void CheckDataString(out string targetString, BinaryReader reader, int buffer, string expected)
        {
            var output = new string(reader.ReadChars(buffer));
            if (output != expected) throw new NotSupportedException("ENGINE only supports RIFF format Wave files");
            targetString = output;
        }
    }

    public class AudioService
    {
        private AudioContext _driver;
        private bool _connected;
        private List<SoundBase> _soundLibrary;
        private readonly double _holdTime; //Time to hold onto sounds before disposing
        public AudioService(double holdTime = 5f)
        {
            _holdTime = holdTime;
        }

        public void Connect()
        {
            _driver = new AudioContext();
            _soundLibrary = new List<SoundBase>();
            _connected = true;
        }

        public SoundBase LoadSound(string filename, bool looping = false)
        {
            if (!_connected) return null;
            var sound = new SoundBase(filename, looping);
            _soundLibrary.Add(sound);
            return sound;
        }

        public void Validate(double dtime)
        {
            if (!_connected) return;

            var n = 0;
            while (n < _soundLibrary.Count)
            {
                var sound = _soundLibrary[n];
                if (sound.State() != ALSourceState.Stopped)
                {
                    n++;
                    continue;
                }

                if (sound.Looping)
                {
                    sound.Play();
                    n++;
                    continue;
                }

                if (!sound.Played)
                {
                    n++;
                    continue;
                }

                sound.Dispose();
                _soundLibrary.Remove(sound);
            }
        }

        public void Reset()
        {
            foreach (var sound in _soundLibrary)
            {
                sound.Dispose();
            }
            _soundLibrary = new List<SoundBase>();
        }

        public void Dispose()
        {
            Reset();
            _driver.Dispose();
            _connected = false;
        }

    }

    public class SoundBase
    {
        public string FileName { get; private set; }
        private readonly int _bufferId;
        private readonly int _sourceId;
        public bool Played;
        public double _holdTime;

        public bool Looping { get; set; }

        public SoundBase(string fileName, bool looping = false)
        {
            FileName = fileName;
            _bufferId = AL.GenBuffer();
            _sourceId = AL.GenSource();
            Looping = looping;
            _holdTime = 0;
            var reader = new WaveReader(fileName);
            AL.BufferData(_bufferId, reader.SoundFormat, reader.Data, reader.Data.Length, reader.SampleRate);
            AL.Source(_sourceId, ALSourcei.Buffer, _bufferId);
        }

        public SoundBase Play()
        {
            AL.SourcePlay(_sourceId);
            Played = true;
            _holdTime = 0;
            return this;
        }

        public SoundBase Pause()
        {
            AL.SourcePause(_sourceId);
            return this;
        }

        public void Dispose()
        {
            AL.SourceStop(_sourceId);
            AL.DeleteSource(_sourceId);
            AL.DeleteBuffer(_bufferId);
        }

        public ALSourceState State()
        {
            return AL.GetSourceState(_sourceId);
        }

        /// <summary>
        /// Decays the Hold Time of the sound. Call it once it stops.
        /// </summary>
        /// <param name="dTime"></param>
        public void Tick(double dTime)
        {
            _holdTime += dTime;
        }

        public bool Complete(double holdTime)
        {
            return _holdTime >= holdTime;
        }
    }
}
