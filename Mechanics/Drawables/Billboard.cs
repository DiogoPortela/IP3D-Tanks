using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TankProject
{
    class Billboard
    {
        internal VertexPositionTexture[] vertexes;
        internal float size;
        internal static short[] indexes = new short[6] {1, 0, 2, 1, 2, 3};

        //--------------------Constructors--------------------//
        internal Billboard(float size)
        {
            vertexes = new VertexPositionTexture[4];
            vertexes[0] = new VertexPositionTexture(new Vector3(-size, size, 0), Vector2.Zero);
            vertexes[1] = new VertexPositionTexture(new Vector3(size, size, 0), new Vector2(1, 0));
            vertexes[2] = new VertexPositionTexture(new Vector3(-size,- size, 0), new Vector2(0, 1));
            vertexes[3] = new VertexPositionTexture(new Vector3(size,- size, 0), Vector2.One);

            this.size = size;
        }

        //--------------------Functions--------------------//
        internal void Draw(GraphicsDevice device)
        {
            device.DrawUserIndexedPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertexes, 0, 4, indexes, 0, 2);
        }
    }
}
