using OpenTK.Graphics.OpenGL4;

namespace FallingSandSimulator;

public abstract class Mesh
{
    public readonly int vertexCount;

    private int vertexBufferObject;

    private int vertexArrayObject;

    protected float[] vertices;

    protected Mesh(int vertexCount, float[] vertices)
    {
        this.vertexCount = vertexCount;
        this.vertices = vertices;
    }

    protected void SetUp()
    {
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