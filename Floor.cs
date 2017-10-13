using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TankProject
{
    static class Floor
    {
        private static VertexBuffer vertexBuffer;
        private static IndexBuffer indexBuffer;
        private static BasicEffect effect;
        private static Texture2D heightMap;
        private static Texture2D texture;
        internal static float[,] VerticesHeight;


        internal static void Start(Game1 game, Camera camera)
        {
            heightMap = game.Content.Load<Texture2D>("lh3d1");
            texture = game.Content.Load<Texture2D>("sand");

            VerticesHeight = new float[heightMap.Width, heightMap.Height];

            effect = new BasicEffect(Game1.graphics.GraphicsDevice);

            float aspectRatio = (float)Game1.graphics.GraphicsDevice.Viewport.Width / Game1.graphics.GraphicsDevice.Viewport.Height;

            effect.World = Matrix.Identity;
            effect.Projection = camera.ProjectionMatrix;
            effect.LightingEnabled = false;
            effect.VertexColorEnabled = false;
            effect.TextureEnabled = true;
            effect.Texture = texture;

            CreateGeometry();
        }

        private static void CreateGeometry()
        {
            VertexPositionNormalTexture[] vertices;
            short[] indices;

            int vertexCount = heightMap.Width * heightMap.Height;
            int indexcount = heightMap.Width * (heightMap.Height - 1) * 2;
            vertices = new VertexPositionNormalTexture[vertexCount];

            Color[] pixelMap = new Color[vertexCount];
            heightMap.GetData<Color>(pixelMap);

            for (int x = 0, n = 0; x < heightMap.Width; x++)
            {
                for (int z = 0; z < heightMap.Height; z++)
                {
                    vertices[n] = new VertexPositionNormalTexture(new Vector3(x, pixelMap[n].R * 0.05f, z), new Vector3(0,1,0), new Vector2(x % 2, (z + 1) % 2));
                    VerticesHeight[x,z] = pixelMap[n++].R * 0.05f;
                }
            }

            vertexBuffer = new VertexBuffer(Game1.graphics.GraphicsDevice, typeof(VertexPositionNormalTexture), vertexCount, BufferUsage.None);
            vertexBuffer.SetData<VertexPositionNormalTexture>(vertices);


            indices = new short[indexcount];
            for (int x = 0, n = 0; x < heightMap.Width - 1; x++)
            {
                for (int z = 0; z < heightMap.Height; z++)
                {
                    indices[n++] = (short)(heightMap.Height * x + z);
                    indices[n++] = (short)(heightMap.Height * (x + 1) + z);
                }
            }

            indexBuffer = new IndexBuffer(Game1.graphics.GraphicsDevice, typeof(short), indexcount, BufferUsage.None);
            indexBuffer.SetData<short>(indices);

        }

        internal static void Draw(Camera camera)
        {
            effect.View = camera.ViewMatrix;

            Game1.graphics.GraphicsDevice.SetVertexBuffer(vertexBuffer);
            Game1.graphics.GraphicsDevice.Indices = indexBuffer;

            effect.CurrentTechnique.Passes[0].Apply();
            for (int i = 0; i < heightMap.Width - 1; i++)
            {
                Game1.graphics.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleStrip, 0, i * heightMap.Width * 2, heightMap.Height * 2 - 2);
            }
        }
    }
}
