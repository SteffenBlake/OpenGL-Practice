using System;
using System.Collections.Generic;
using OpenGL_Practice.Models.Classes;
using OpenGL_Practice.Services.Classes;

namespace OpenGL_Practice.Models
{
    public class Doge : ModelBase
    {
        protected override Dictionary<string, Animation> Animations() => new Dictionary<string, Animation>{ {"Base", Animation.FromString("Doge.png") } };

        protected override bool Loops() => false;
    }
}
