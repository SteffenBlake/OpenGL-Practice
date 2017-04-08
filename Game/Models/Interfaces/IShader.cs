namespace OpenGL_Practice.Models.Interfaces
{
    public interface IShader
    {
        int Attribute(string name);
        uint Buffer(string name);
        void DisableVertexAttribArrays();
        void EnableVertexAttribArrays();
        void GenBuffers();
        void Link();
        int Uniform(string name);
    }
}