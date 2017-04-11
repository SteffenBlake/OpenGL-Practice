using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using OpenGL_Practice.Models;
using OpenGL_Practice.Models.Classes;
using OpenGL_Practice.Services.Classes;
using OpenTK;
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

        private Random rnd = new Random();

        private Doge doge;
        private Vector2 dogeSpeed;

        protected override void SubscribeEvents()
        {
            InputService.Subscribe(Key.Escape, EndProgram);
            InputService.Subscribe(Key.Down, Down);
        }

        protected override void LoadModels()
        {
            doge = new Doge(rnd.Next(300, 500), rnd.Next(200, 400));
            Models.Add(doge);
            dogeSpeed = new Vector2(2, 2);
        }

        private void EndProgram()
        {
            Exit();
        }

        private void Down()
        {
            Console.WriteLine("Pressed down");
        }

        protected override void OnRenderFrame(FrameEventArgs events)
        {
            base.OnRenderFrame(events);

            if (doge.Model.Position.X - doge.Model.HalfWidth <= 0 || doge.Model.Position.X + doge.Model.HalfWidth >= Width)
            {
                dogeSpeed = new Vector2(-dogeSpeed.X, dogeSpeed.Y);
                AudioService.LoadSound("bork.wav").Play();
            }

            if (doge.Model.Position.Y - doge.Model.HalfHeight <= 0 || doge.Model.Position.Y + doge.Model.HalfHeight >= Height)
            {
                dogeSpeed = new Vector2(dogeSpeed.X, -dogeSpeed.Y);
                AudioService.LoadSound("bork.wav").Play();
            }

            doge.Model.Slide(dogeSpeed);

        }

        protected override void OnLoad(EventArgs events)
        {
            base.OnLoad(events);
            AudioService.LoadSound("moon.wav", true).Play();
        }
    }
}
