using OpenTK.Graphics.OpenGL4;

namespace FallingSandSimulator;

public abstract class Mesh
{
    public readonly int vertexCount;

    private readonly int vertexBufferObject;

    private readonly int vertexArrayObject;

    protected Mesh(int vertexCount)
    {
        this.vertexCount = vertexCount;
        vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(vertexArrayObject);

        vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
    }


    public void Draw()
    {
        GL.BindVertexArray(vertexArrayObject);
        GL.DrawArrays(PrimitiveType.Triangles, 0, vertexCount);
    }

    public void Delete()
    {
        GL.DeleteBuffer(vertexBufferObject);
        GL.DeleteVertexArray(vertexArrayObject);
    }
}