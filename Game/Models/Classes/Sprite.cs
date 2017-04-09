using OpenGL_Practice.Services.Classes;
using OpenTK;

namespace OpenGL_Practice.Models.Classes
{
    public class Sprite
    {
        public Sprite(string fileName)
        {
            TextureId = TextureService.GetTexture(Assets.GetImage(fileName));
            Size = TextureService.GetSize(TextureId);
        }
        public Vector2 Size { get; set; }
        public int TextureId { get; set; }
    }
}
