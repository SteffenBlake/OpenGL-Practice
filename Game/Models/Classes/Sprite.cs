using OpenGL_Practice.Services.Classes;
using OpenTK;

namespace OpenGL_Practice.Models.Classes
{
    public class Sprite
    {
        public Sprite(string fileName, TextureService service)
        {
            TextureId = service.GetTexture(Assets.GetImage(fileName));
            Size = service.GetSize(TextureId);
        }
        public Vector2 Size { get; set; }
        public int TextureId { get; set; }
    }
}
