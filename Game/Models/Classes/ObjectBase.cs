using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenGL_Practice.Models.Interfaces;
using OpenGL_Practice.Services.Classes;
using OpenTK;

namespace OpenGL_Practice.Models.Classes
{
    public class ObjectBase : IObjectBase
    {
        private readonly Dictionary<string, Animation> _animations;
        private Animation _currentAnimation;
        private readonly float _frameRate;
        private readonly bool _loops;
        private readonly IAlignment _alignemt;

        public ObjectBase(Dictionary<string, Animation> animations, IAlignment alignemt = null, float frameRate = 0.5f, bool loops = true,
            string initialAnimation = "", bool pause = true, bool visible = true,
            float x = 0, float y = 0, float xScale = 0, float yScale = 0, float rotation = 0)
        {
            _animations = animations;
            _alignemt = alignemt;
            _frameRate = frameRate;
            _loops = loops;
            Visible = visible;

            _currentAnimation = initialAnimation != "" ? animations[initialAnimation] : animations.Values.First();
            if (pause) _currentAnimation.Pause();

            if (alignemt == null) alignemt = Alignments.TrueCenter;
            _alignemt = alignemt;

            Position = new Vector2(x, y);
            Scale = new Vector2(xScale, yScale);
            Rotation = rotation;
        }

        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }
        public float Rotation { get; set; }

        public bool Visible { get; set; }

        public bool Paused => _currentAnimation.Paused;

        public Sprite CurrentSprite() => _currentAnimation.CurrentSprite();

        public void AddAnimation(string animationName, Animation animation)
        {
            _animations[animationName] = animation;
        }

        public void AddAnimation(string animationName, IList<Sprite> animation )
        {
            _animations[animationName] = new Animation(animation, _loops, _frameRate);
        }

        public void SetAnimation(string animationName, bool reset = true)
        {
            _currentAnimation = _animations[animationName];
            if (reset) _currentAnimation.Reset();
        }

        public void Pause()
        {
            _currentAnimation.Pause();
        }

        public void Play()
        {
            _currentAnimation.Play();
        }

        public void Reset()
        {
            _currentAnimation.Reset();
        }

        public void Step(double seconds)
        {
            _currentAnimation.Step(seconds);
        }

        public Matrix4 ModelViewProjectionMatrix = Matrix4.Identity;

        public IEnumerable<Vector2> Vertices()
        {
            return new [] {
                new Vector2(-0.5f, -0.5f),
                new Vector2(-0.5f,  0.5f),
                new Vector2(0.5f,  0.5f),
                new Vector2(0.5f, -0.5f)
            };
        }

        private readonly RectangleF _coordRectangle = new RectangleF(0.0f, 0.0f, 1.0f, 1.0f);
        public Vector2[] Coordinates()
        {
            return new [] {
                new Vector2(_coordRectangle.Left, _coordRectangle.Bottom),
                new Vector2(_coordRectangle.Left,  _coordRectangle.Top),
                new Vector2(_coordRectangle.Right, _coordRectangle.Top),
                new Vector2(_coordRectangle.Right, _coordRectangle.Bottom)
            };
        }

        public IEnumerable<int> Indices(int offset)
        {
            var indices = new [] { 0, 1, 2, 0, 2, 3 };

            if (offset == 0) return indices;

            for (int i = 0, max = indices.Length; i < max; i++)
            {
                indices[i] += offset;
            }

            return indices;
        }

        public Matrix4 Matrix { get; set; }
        public void CalculateMatrix(int windowWidth, int windowHeight, Vector2? viewPos = null, int layer = 0)
        {
            if (viewPos == null) viewPos = new Vector2(0, 0);

            var translation = new Vector3(Position.X - windowWidth / 2f - viewPos.Value.X, Position.Y - windowHeight / 2f - viewPos.Value.Y, layer);

            Matrix = Matrix4.CreateScale(Size.X, Size.Y, 1.0f) * Matrix4.CreateRotationZ(Rotation) * Matrix4.CreateTranslation(translation);
        }

        public void Rotate(float degrees)
        {
            Rotation += degrees;
        }

        public void Slide(float x = 0, float y = 0)
        {
            Position = new Vector2(Position.X + x, Position.Y + y);
        }

        public void Slide(Vector2 distance)
        {
            Position = new Vector2(Position.X + distance.X, Position.Y + distance.Y);
        }

        public void Stretch(float x = 1, float y = 1)
        {
            Scale = new Vector2(Scale.X * x, Scale.Y * y);
        }

        public Vector2 Size => new Vector2(CurrentSprite().Size.X * Scale.X, CurrentSprite().Size.Y * Scale.Y);
    }
}
