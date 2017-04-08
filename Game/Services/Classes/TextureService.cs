using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace OpenGL_Practice.Services.Classes
{
    public class TextureService
    {
        private readonly Dictionary<string, int> _textureDict = new Dictionary<string,int>();
        private readonly Dictionary<int, Vector2> _sizeDict = new Dictionary<int, Vector2>();

        public int GetTexture(string fileName)
        {
            if (_textureDict.ContainsKey(fileName)) return _textureDict[fileName];


            var image = new Bitmap(Image.FromFile(Assets.GetImage("Doge.bmp")));
            var texId = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, texId);

            var data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            image.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int) TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int) TextureMagFilter.Linear);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            _textureDict[fileName] = texId;
            _sizeDict[texId] = new Vector2(image.Width, image.Height);

            return texId;
        }

        public Vector2 GetSize(int textureId)
        {
            return _sizeDict[textureId];
        }
    }
}