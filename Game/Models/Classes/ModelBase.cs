using System.Collections.Generic;
using System.Linq;
using OpenGL_Practice.Services.Classes;

namespace OpenGL_Practice.Models.Classes
{
    public abstract class ModelBase
    {
        public ObjectBase Model { get; set; }

        protected TextureService TextureService;

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

        protected ModelBase(TextureService textureService)
        {
            TextureService = textureService;
            Model = new ObjectBase(Animations(), Alignment(), FrameRate(), Loops(), InitialAnimation(), Pause(), Visible(), X(), Y(), XScale(), YScale(), Rotation());
        }
    }
}
