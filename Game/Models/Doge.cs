using System;
using System.Collections.Generic;
using OpenGL_Practice.Models.Classes;
using OpenGL_Practice.Services.Classes;

namespace OpenGL_Practice.Models
{
    public class Doge : ModelBase
    {
        public Doge(TextureService textureService) : base(textureService)
        {
        }

        protected override Dictionary<string, Animation> Animations() => new Dictionary<string, Animation>{ {"Base", Animation.FromString("Doge.png", TextureService) } };

        protected override bool Loops() => false;

        protected override float X() => 400;
        protected override float Y() => 320;
    }
}
