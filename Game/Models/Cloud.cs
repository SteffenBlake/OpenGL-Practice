using System.Collections.Generic;
using OpenGL_Practice.Models.Classes;

namespace OpenGL_Practice.Models
{
    public class Cloud : ModelBase
    {
        public Cloud(float? x = null, float? y = null, float? xScale = null, float? yScale = null, float? rotation = null) 
            : base(x, y, xScale, yScale, rotation) { }

        protected override Dictionary<string, Animation> Animations() => new Dictionary<string, Animation>{ {"Base", Animation.FromString("cloud1.png") } };

        protected override bool Loops() => false;
    }
}
