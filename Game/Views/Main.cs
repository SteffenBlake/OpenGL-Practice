using System;
using System.Collections.Generic;
using System.Drawing;
using OpenGL_Practice.Models;
using OpenGL_Practice.Models.Classes;
using OpenTK.Input;

namespace OpenGL_Practice.Views
{
    public class Main : WindowBase
    {
        protected override int _defaultWidth() => 800;
        protected override int _defaultHeight() => 640;
        protected override string _defaultTitle() => "Doge";
        protected override string _defaultIcon() => "doge.ico";
        protected override string _defaultVertShader() => "default.vert";
        protected override string _defaultFragShader() => "default.frag";
        protected override Color _defaultBackground() => Color.CornflowerBlue;

        protected override void SubscribeEvents()
        {
            InputService.Subscribe(Key.Escape, EndProgram);
            InputService.Subscribe(Key.Down, Down);
        }

        protected override void LoadModels()
        {
            var rnd = new Random();
            Models.Add(new Doge());
            for (var n = 0; n <= 500; n++)
            {
                float x = rnd.Next(-800, 800);
                float y = rnd.Next(-640, 640);
                Models.Add(new Cloud(x, y));
            }
        }

        private void EndProgram()
        {
            Exit();
        }

        private void Down()
        {
            Console.WriteLine("Pressed down");
        }
    }
}
