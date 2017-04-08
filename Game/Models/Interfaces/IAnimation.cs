namespace OpenGL_Practice.Models.Interfaces
{
    public interface IAnimation
    {
        bool Paused { get; }

        ISprite CurrentSprite();
        void Pause();
        void Play();
        void Reset();
        void Step(float seconds);
    }
}