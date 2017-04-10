using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace OpenGL_Practice.Services.Classes
{
    public static class TextureService
    {
        private static readonly Dictionary<string, int> TextureDict = new Dictionary<string,int>();
        private static readonly Dictionary<int, Vector2> SizeDict = new Dictionary<int, Vector2>();

        public static int GetTexture(string fileName)
        {
            if (TextureDict.ContainsKey(fileName)) return TextureDict[fileName];


            var image = new Bitmap(Image.FromFile(Assets.GetImage(fileName)));
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

            TextureDict[fileName] = texId;
            SizeDict[texId] = new Vector2(image.Width, image.Height);

            return texId;
        }

        public static Vector2 GetSize(int textureId)
        {
            return SizeDict[textureId];
        }
    }
}