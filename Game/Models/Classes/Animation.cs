using System;
using System.Collections.Generic;
using OpenGL_Practice.Models.Interfaces;
using OpenGL_Practice.Services.Classes;

namespace OpenGL_Practice.Models.Classes
{
    public class Animation
    {
        private double _currentTime;
        private readonly double _frameRate;
        private readonly bool _loops;

        private readonly IList<Sprite> _sprites;


        public bool Paused { get; private set; }

        public Animation(IList<Sprite> sprites, bool loops = true, double frameRate = 0.5, bool paused = true)
        {
            _sprites = sprites;
            _loops = loops;
            _frameRate = frameRate;
            _currentTime = 0f;
            Paused = paused;
        }

        public static Animation FromString(string fileName, TextureService service, bool loops = true, double frameRate = 0.5, bool paused = true)
        {
            var sprites = new List<Sprite> { new Sprite(fileName, service) };
            return new Animation(sprites, loops, frameRate, paused);
        }

        private double AnimationLength => _sprites.Count * _frameRate;

        public void Step(double seconds)
        {
            
            if (!_loops && _currentTime >= AnimationLength || Paused) return;

            _currentTime += seconds;
            _currentTime = _currentTime % AnimationLength;
        }

        public void Pause()
        {
            Paused = true;
        }

        public void Play()
        {
            Paused = false;
        }

        public void Reset()
        {
            _currentTime = 0;
        }

        public Sprite CurrentSprite()
        {
            return _sprites[(int) Math.Round(_currentTime / _frameRate)];
        }

    }
}
