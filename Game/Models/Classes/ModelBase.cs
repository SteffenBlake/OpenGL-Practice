using System.Collections.Generic;
using System.Linq;
using OpenGL_Practice.Services.Classes;
using OpenTK;

namespace OpenGL_Practice.Models.Classes
{
    public abstract class ModelBase
    {
        public ObjectBase Model { get; set; }
        protected abstract Dictionary<string, Animation> Animations();
        protected virtual IAlignment Alignment() => Alignments.TrueCenter;
        protected virtual float FrameRate() => 0.5f;
        protected virtual bool Loops() => true;
        protected virtual string InitialAnimation() => "";
        protected virtual bool Pause() => false;
        protected virtual bool Visible() => true;
        protected virtual float X() => 0.0f;
        protected virtual float Y() => 0.0f;
        protected virtual float XScale() => 1f;
        protected virtual float YScale() => 1f;
        protected virtual float Rotation() => 0.0f;

        protected ModelBase(float? x, float? y, float? xScale, float? yScale, float? rotation)
        {
            Model = new ObjectBase(Animations(), Alignment(), FrameRate(), Loops(), InitialAnimation(), Pause(), Visible(), X(), Y(), XScale(), YScale(), Rotation());
            if (x.HasValue) Model.Position = new Vector2(x.Value, Model.Position.Y);
            if (y.HasValue) Model.Position = new Vector2(Model.Position.X, y.Value);
            if (xScale.HasValue) Model.Scale = new Vector2(xScale.Value, Model.Scale.Y);
            if (yScale.HasValue) Model.Scale = new Vector2(Model.Scale.X, yScale.Value);
            if (rotation.HasValue) Model.Rotation = rotation.Value;
        }
    }
}
