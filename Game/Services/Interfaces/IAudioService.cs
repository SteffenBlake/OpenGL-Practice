namespace OpenGL_Practice.Services.Interfaces
{
    public interface IAudioService
    {
        void Dispose();
        void PlaySound(string fileName, bool loops);
    }
}