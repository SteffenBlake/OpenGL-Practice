using System.Collections.Generic;
using OpenGL_Practice.Models.Classes;
using OpenTK;

namespace OpenGL_Practice.Models.Interfaces
{
    public interface IObjectBase
    {
        Matrix4 Matrix { get; set; }
        bool Paused { get; }
        Vector2 Position { get; set; }
        float Rotation { get; set; }
        Vector2 Scale { get; set; }
        bool Visible { get; set; }

        void AddAnimation(string animationName, Animation animation);
        void AddAnimation(string animationName, IList<Sprite> animation);
        void CalculateMatrix(int windowWidth, int windowHeight, Vector2? viewPos = default(Vector2?), int layer = 0);
        Vector2[] Coordinates();
        Sprite CurrentSprite();
        IEnumerable<int> Indices(int offset);
        void Pause();
        void Play();
        void Reset();
        void Rotate(float degrees);
        void SetAnimation(string animationName, bool reset = true);
        void Slide(float x = 0, float y = 0);
        void Step(double seconds);
        void Stretch(float x = 1, float y = 1);
        IEnumerable<Vector2> Vertices();
    }
}