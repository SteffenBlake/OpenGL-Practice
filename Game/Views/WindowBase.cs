using System;
using System.Collections.Generic;
using System.Drawing;
using OpenGL_Practice.Models.Classes;
using OpenGL_Practice.Services.Classes;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace OpenGL_Practice.Views
{
    public abstract class WindowBase : GameWindow
    {
        protected InputService InputService;
        protected TextureService TextureService;
        public RectangleF CurrentView = new RectangleF(0, 0, 800, 600);

        private int _buffers;
        protected List<ModelBase> Models = new List<ModelBase>();
        private Matrix4 _ortho;
        private Shader _shader;
        private bool Updated { get; set; }

        protected WindowBase() : base(1, 1, new OpenTK.Graphics.GraphicsMode(new OpenTK.Graphics.ColorFormat(8, 8, 8, 8), 3, 3, 4), "WINDOW", GameWindowFlags.Default, DisplayDevice.Default, 3, 0, OpenTK.Graphics.GraphicsContextFlags.ForwardCompatible)
        {
            InputService = new InputService();
            TextureService = new TextureService();
            Run(60, 60);
        }

        protected abstract int _defaultWidth();
        protected abstract int _defaultHeight();
        protected abstract string _defaultTitle();
        protected abstract string _defaultIcon();
        protected abstract string _defaultVertShader();
        protected abstract string _defaultFragShader();
        protected abstract Color _defaultBackground();

        protected abstract void SubscribeEvents();
        protected abstract void LoadModels();

        protected override void OnLoad(EventArgs events)
        {
            base.OnLoad(events);

            Width = _defaultWidth();
            Height = _defaultHeight();
            Title = _defaultTitle();
            Icon = new Icon(Assets.GetImage(_defaultIcon()));
            _shader = new Shader(_defaultVertShader(), _defaultFragShader());

            SubscribeEvents();
            LoadModels();

            GL.ClearColor(_defaultBackground());
            GL.Viewport(0, 0, Width, Height);

            GL.UseProgram(_shader.ProgramId);
            GL.GenBuffers(1, out _buffers);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
        }

        protected override void OnRenderFrame(FrameEventArgs events)
        {
            if (!Updated) return;
            base.OnRenderFrame(events);

            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            var offset = 0;

            GL.UseProgram(_shader.ProgramId);
            _shader.EnableVertexAttribArrays();

            foreach (var model in Models)
            {
                if (!model.Model.Visible) continue;

                GL.BindTexture(TextureTarget.Texture2D, model.Model.CurrentSprite().TextureId);
                GL.UniformMatrix4(_shader.Uniform("mvp"), false, ref model.Model.ModelViewProjectionMatrix);
                GL.Uniform1(_shader.Attribute("mytexture"), model.Model.CurrentSprite().TextureId);
                GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedInt, offset * sizeof(uint));
                offset += 6;
            }

            _shader.DisableVertexAttribArrays();

            GL.Flush();
            SwapBuffers();
        }
        protected override void OnUpdateFrame(FrameEventArgs events)
        {
            base.OnUpdateFrame(events);
            var vertices = new List<Vector2>();
            var coordinates = new List<Vector2>();
            var indices = new List<int>();

            var offset = 0;

            foreach (var model in Models)
            {
                model.Model.Step(events.Time);
                if (!model.Model.Visible) continue;
                vertices.AddRange(model.Model.Vertices());
                coordinates.AddRange(model.Model.Coordinates());
                indices.AddRange(model.Model.Indices(offset));
                offset += 4;

                model.Model.CalculateMatrix(Width, Height);
                model.Model.ModelViewProjectionMatrix = model.Model.Matrix * _ortho;
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, _shader.Buffer("v_coord"));

            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Count * Vector2.SizeInBytes), vertices.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(_shader.Attribute("v_coord"), 2, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _shader.Buffer("v_texcoord"));
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(coordinates.Count * Vector2.SizeInBytes), coordinates.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(_shader.Attribute("v_texcoord"), 2, VertexAttribPointerType.Float, true, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // Buffer indices
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _buffers);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Count * sizeof(int)), indices.ToArray(), BufferUsageHint.StaticDraw);

            Updated = true;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            _ortho = Matrix4.CreateOrthographic(ClientSize.Width, ClientSize.Height, -1.0f, 2.0f);
            CurrentView.Size = new SizeF(ClientSize.Width, ClientSize.Height);
        }

        protected override void OnMove(EventArgs e)
        {
            base.OnMove(e);
            InputService.Wipe();
        }

        protected override void OnFocusedChanged(EventArgs e)
        {
            base.OnFocusedChanged(e);
            InputService.Wipe();
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs key)
        {
            base.OnKeyDown(key);
            InputService.HandleKey(key.Key, true);
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs key)
        {
            base.OnKeyDown(key);
            InputService.HandleKey(key.Key, false);
        }
    }
}