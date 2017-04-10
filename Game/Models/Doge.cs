using System;
using System.Collections.Generic;
using OpenGL_Practice.Models.Classes;
using OpenGL_Practice.Services.Classes;

namespace OpenGL_Practice.Models
{
    public class Doge : ModelBase
    {
        public Doge(float? x = null, float? y = null, float? xScale = null, float? yScale = null, float? rotation = null) : base(x, y, xScale, yScale, rotation) { }

        protected override Dictionary<string, Animation> Animations() => new Dictionary<string, Animation>{ {"Base", Animation.FromString("Doge.png") } };

        protected override bool Loops() => false;

        protected override float XScale() => -1f;
    }
}
