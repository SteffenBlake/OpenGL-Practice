#pragma warning disable 168
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenGL_Practice.Models.Interfaces;
using OpenGL_Practice.Services.Classes;
using OpenTK.Graphics.OpenGL4;
namespace OpenGL_Practice.Models.Classes
{
    public class Shader : IShader
    {
        private readonly Dictionary<string, AttributeBase> _attributes;
        private readonly Dictionary<string, UniformBase> _uniforms;
        private readonly Dictionary<string, uint> _buffers = new Dictionary<string, uint>();

        public readonly int ProgramId;
        private int _attributeCount;
        private int _uniformCount;

        public Shader(string vertexShader, string fragmentShader)
        {
            ProgramId = GL.CreateProgram();
            _attributes = new Dictionary<string, AttributeBase>();
            _uniforms = new Dictionary<string, UniformBase>();
            LoadShader(vertexShader, ShaderType.VertexShader, out int vShaderId);
            LoadShader(fragmentShader, ShaderType.FragmentShader, out int fShaderId);
            Link();
            GenBuffers();
        }

        private void LoadShader(string filename, ShaderType type, out int address)
        {
            address = GL.CreateShader(type);
            GL.ShaderSource(address, Assets.GetShader(filename));
            GL.CompileShader(address);
            GL.AttachShader(ProgramId, address);
        }

        public void Link()
        {
            GL.LinkProgram(ProgramId);

            Console.WriteLine(GL.GetProgramInfoLog(ProgramId));

            GL.GetProgram(ProgramId, GetProgramParameterName.ActiveAttributes, out _attributeCount);
            GL.GetProgram(ProgramId, GetProgramParameterName.ActiveUniforms, out _uniformCount);

            for (var i = 0; i < _attributeCount; i++)
            {
                var info = new AttributeBase();
                var name = new StringBuilder();

                GL.GetActiveAttrib(ProgramId, i, 256, out int length, out info.Size, out info.Type, name);

                info.Name = name.ToString();
                info.Address = GL.GetAttribLocation(ProgramId, info.Name);
                _attributes[name.ToString()] = info;
            }

            for (var i = 0; i < _uniformCount; i++)
            {
                var info = new UniformBase();

                var name = new StringBuilder();

                GL.GetActiveUniform(ProgramId, i, 256, out int length, out info.Size, out info.Type, name);

                info.Name = name.ToString();
                info.Address = GL.GetUniformLocation(ProgramId, info.Name);
                _uniforms[name.ToString()] = info;
            }
        }

        public void GenBuffers()
        {
            foreach (var name in _attributes.Values.Concat<InfoBase>(_uniforms.Values).Select(i => i.Name))
            {
                GL.GenBuffers(1, out uint buffer);
                _buffers[name] = buffer;
            }
        }

        public void EnableVertexAttribArrays()
        {
            foreach (var address in _attributes.Values.Select(a => a.Address))
            {
                GL.EnableVertexAttribArray(address);
            }
        }

        public void DisableVertexAttribArrays()
        {
            foreach (var address in _attributes.Values.Select(a => a.Address))
            {
                GL.DisableVertexAttribArray(address);
            }
        }

        public int Attribute(string name)
        {
            return _attributes.ContainsKey(name) ? _attributes[name].Address : -1;
        }

        public int Uniform(string name)
        {
            return _uniforms.ContainsKey("name") ? _uniforms[name].Address : -1;
        }

        public uint Buffer(string name)
        {
            return _buffers.ContainsKey(name) ? _buffers[name] : 0;
        }
    }

    public abstract class InfoBase
    {
        public string Name;
        public int Address;
        public int Size;

        protected InfoBase(string name = "", int address = -1, int size = 0)
        {
            Name = name;
            Address = address;
            Size = size;
        }
    }

    public class UniformBase : InfoBase
    {
        public ActiveUniformType Type;
    }

    public class AttributeBase : InfoBase
    {
        public ActiveAttribType Type;
    }
}
