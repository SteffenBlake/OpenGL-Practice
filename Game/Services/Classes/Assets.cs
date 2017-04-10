using System.Collections.Generic;
using System.IO;
using System.Media;

namespace OpenGL_Practice.Services.Classes
{
    public static class Assets
    {
        private static readonly string CurrentDir = Directory.GetCurrentDirectory() + "\\Assets\\";
        public static string GetImage(string fileName) => CurrentDir + "Images\\" + fileName;

        public static string GetShader(string fileName)
        {
            using (var reader = new StreamReader(CurrentDir + "Shaders\\" + fileName))
            {
                return reader.ReadToEnd();
            }
        }

        public static string GetSound(string fileName) => CurrentDir + "Sounds\\" + fileName;
    }
}
