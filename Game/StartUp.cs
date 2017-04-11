using System.Linq.Expressions;
using OpenGL_Practice.Services.Classes;
using OpenGL_Practice.Views;

namespace OpenGL_Practice
{
    public static class StartUp
    {

        public static void Main()
        {
            var main = new Main();
            main.Run(60,60);
        }
    }
}
