using OpenTK;

namespace OpenGL_Practice.Models.Interfaces
{
    public interface ISprite
    {
        Vector2 Size { get; set; }
        int TextureId { get; set; }
    }
}