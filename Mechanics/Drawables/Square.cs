using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TankProject
{
    class Square
    {
        internal VertexPositionTexture[] vertexes;
        internal float size;
        internal static short[] indexes = new short[12] { 0, 1, 2, 1, 3, 2 /**/,1, 0, 2, 1, 2, 3};

        internal Square(Vector3 position, Texture texture, float size)
        {
            vertexes = new VertexPositionTexture[4];
            vertexes[0] = new VertexPositionTexture(new Vector3(position.X - size, position.Y + size, position.Z), Vector2.Zero);
            vertexes[1] = new VertexPositionTexture(new Vector3(position.X + size, position.Y + size, position.Z), new Vector2(1,0));
            vertexes[2] = new VertexPositionTexture(new Vector3(position.X - size, position.Y - size, position.Z), new Vector2(0, 1));
            vertexes[3] = new VertexPositionTexture(new Vector3(position.X + size, position.Y - size, position.Z), Vector2.One);
            this.size = size;
        }

        internal void Update(Vector3 position)
        {
            vertexes[0].Position = new Vector3(position.X - size, position.Y + size, position.Z);
            vertexes[1].Position = new Vector3(position.X + size, position.Y + size, position.Z);
            vertexes[2].Position = new Vector3(position.X - size, position.Y - size, position.Z);
            vertexes[3].Position = new Vector3(position.X + size, position.Y - size, position.Z);
        }

        internal void Draw(GraphicsDevice device)
        {
            device.DrawUserIndexedPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertexes, 0, 4, indexes, 0, 4);
        }
    }
}
