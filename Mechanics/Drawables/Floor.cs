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
        internal static Vector3[,] VerticesNormals;

        //--------------------Functions--------------------//

        internal static void Start(Game1 game, Camera camera, Material material, Light light)
        {
            heightMap = game.Content.Load<Texture2D>("lh3d1");
            texture = game.Content.Load<Texture2D>("sand");

            VerticesHeight = new float[heightMap.Width, heightMap.Height];
            VerticesNormals = new Vector3[heightMap.Width, heightMap.Height];

            effect = new BasicEffect(Game1.graphics.GraphicsDevice);

            float aspectRatio = (float)Game1.graphics.GraphicsDevice.Viewport.Width / Game1.graphics.GraphicsDevice.Viewport.Height;

            effect.World = Matrix.Identity;
            effect.Projection = camera.ProjectionMatrix;

            effect.TextureEnabled = true;
            effect.Texture = texture;

            //Material
            effect.AmbientLightColor = material.AmbientColor;
            effect.DiffuseColor = material.DiffuseColor;
            effect.SpecularColor = material.SpecularColor;
            effect.SpecularPower = material.SpecularPower;
            effect.PreferPerPixelLighting = true;

            //Light
            effect.LightingEnabled = true;
            effect.DirectionalLight0.Enabled = true;
            effect.DirectionalLight0.DiffuseColor = light.DiffuseColor;
            effect.DirectionalLight0.SpecularColor = light.SpecularColor;
            effect.DirectionalLight0.Direction = light.Direction;

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
                    vertices[n] = new VertexPositionNormalTexture(new Vector3(x, pixelMap[n].R * 0.02f, z), new Vector3(0, 1, 0), new Vector2(x, z));
                    VerticesHeight[x, z] = pixelMap[n++].R * 0.02f;
                }
            }

            //Calculo das normais
            //Miolo
            for (int x = 1; x < heightMap.Width - 1; x++)
            {
                for (int z = 1; z < heightMap.Height - 1; z++)
                {
                    Vector3 currentPosition = vertices[x * heightMap.Height + z].Position;
                    Vector3 normal1 = Vector3.Cross(vertices[(x - 1) * heightMap.Height + (z - 1)].Position - currentPosition, vertices[(x - 1) * heightMap.Height + z].Position - currentPosition);
                    Vector3 normal2 = Vector3.Cross(vertices[(x - 1) * heightMap.Height + z].Position - currentPosition, vertices[(x - 1) * heightMap.Height + (z + 1)].Position - currentPosition);
                    Vector3 normal3 = Vector3.Cross(vertices[(x - 1) * heightMap.Height + (z + 1)].Position - currentPosition, vertices[x * heightMap.Height + (z + 1)].Position - currentPosition);
                    Vector3 normal4 = Vector3.Cross(vertices[x * heightMap.Height + (z + 1)].Position - currentPosition, vertices[(x + 1) * heightMap.Height + (z + 1)].Position - currentPosition);
                    Vector3 normal5 = Vector3.Cross(vertices[(x + 1) * heightMap.Height + (z + 1)].Position - currentPosition, vertices[(x + 1) * heightMap.Height + z].Position - currentPosition);
                    Vector3 normal6 = Vector3.Cross(vertices[(x + 1) * heightMap.Height + z].Position - currentPosition, vertices[(x + 1) * heightMap.Height + (z - 1)].Position - currentPosition);
                    Vector3 normal7 = Vector3.Cross(vertices[(x + 1) * heightMap.Height + (z - 1)].Position - currentPosition, vertices[x * heightMap.Height + (z - 1)].Position - currentPosition);
                    Vector3 normal8 = Vector3.Cross(vertices[x * heightMap.Height + (z - 1)].Position - currentPosition, vertices[(x - 1) * heightMap.Height + (z - 1)].Position - currentPosition);
                    normal1.Normalize();
                    normal2.Normalize();
                    normal3.Normalize();
                    normal4.Normalize();
                    normal5.Normalize();
                    normal6.Normalize();
                    normal7.Normalize();
                    normal8.Normalize();
                    vertices[x * heightMap.Height + z].Normal = (normal1 + normal2 + normal3 + normal4 + normal5 + normal6 + normal7 + normal8) / 8.0f;
                    VerticesNormals[x, z] = (normal1 + normal2 + normal3 + normal4 + normal5 + normal6 + normal7 + normal8) / 8.0f;
                    VerticesNormals[x, z].Normalize();

                }
            }
            // Top
            for (int x = 1; x < heightMap.Width - 1; x++)
            {
                Vector3 currentPosition = vertices[x * heightMap.Height].Position;
                Vector3 normal1 = Vector3.Cross(vertices[(x - 1) * heightMap.Height].Position - currentPosition, vertices[(x - 1) * heightMap.Height + 1].Position - currentPosition);
                Vector3 normal2 = Vector3.Cross(vertices[(x - 1) * heightMap.Height + 1].Position - currentPosition, vertices[x * heightMap.Height + 1].Position - currentPosition);
                Vector3 normal3 = Vector3.Cross(vertices[x * heightMap.Height + 1].Position - currentPosition, vertices[(x + 1) * heightMap.Height + 1].Position - currentPosition);
                Vector3 normal4 = Vector3.Cross(vertices[(x + 1) * heightMap.Height + 1].Position - currentPosition, vertices[(x + 1) * heightMap.Height].Position - currentPosition);
                normal1.Normalize();
                normal2.Normalize();
                normal3.Normalize();
                normal4.Normalize();
                vertices[x * heightMap.Height].Normal = (normal1 + normal2 + normal3 + normal4) / 4.0f;
                VerticesNormals[x, 0] = (normal1 + normal2 + normal3 + normal4) / 4.0f;
                VerticesNormals[x, 0].Normalize();

            }
            // Bottom
            for (int x = 1; x < heightMap.Width - 1; x++)
            {
                Vector3 currentPosition = vertices[x * heightMap.Height + (heightMap.Height - 1)].Position;
                Vector3 normal1 = Vector3.Cross(vertices[(x + 1) * heightMap.Height + (heightMap.Height - 1)].Position - currentPosition, vertices[(x + 1) * heightMap.Height + (heightMap.Height - 2)].Position - currentPosition);
                Vector3 normal2 = Vector3.Cross(vertices[(x + 1) * heightMap.Height + (heightMap.Height - 2)].Position - currentPosition, vertices[x * heightMap.Height + (heightMap.Height - 2)].Position - currentPosition);
                Vector3 normal3 = Vector3.Cross(vertices[x * heightMap.Height + (heightMap.Height - 2)].Position - currentPosition, vertices[(x - 1) * heightMap.Height + (heightMap.Height - 2)].Position - currentPosition);
                Vector3 normal4 = Vector3.Cross(vertices[(x - 1) * heightMap.Height + (heightMap.Height - 2)].Position - currentPosition, vertices[(x - 1) * heightMap.Height + (heightMap.Height - 1)].Position - currentPosition);
                normal1.Normalize();
                normal2.Normalize();
                normal3.Normalize();
                normal4.Normalize();
                vertices[x * heightMap.Height + (heightMap.Height - 1)].Normal = (normal1 + normal2 + normal3 + normal4) / 4.0f;
                VerticesNormals[x, heightMap.Height - 1] = (normal1 + normal2 + normal3 + normal4) / 4.0f;
                VerticesNormals[x, heightMap.Height - 1].Normalize();

            }
            // Left
            for (int z = 1; z < heightMap.Height - 1; z++)
            {
                Vector3 currentPosition = vertices[z].Position;
                Vector3 normal1 = Vector3.Cross(vertices[z + 1].Position - currentPosition, vertices[heightMap.Height + (z + 1)].Position - currentPosition);
                Vector3 normal2 = Vector3.Cross(vertices[heightMap.Height + (z + 1)].Position - currentPosition, vertices[heightMap.Height + z].Position - currentPosition);
                Vector3 normal3 = Vector3.Cross(vertices[heightMap.Height + z].Position - currentPosition, vertices[heightMap.Height + (z - 1)].Position - currentPosition);
                Vector3 normal4 = Vector3.Cross(vertices[heightMap.Height + (z - 1)].Position - currentPosition, vertices[z - 1].Position - currentPosition);
                normal1.Normalize();
                normal2.Normalize();
                normal3.Normalize();
                normal4.Normalize();
                vertices[z].Normal = (normal1 + normal2 + normal3 + normal4) / 4.0f;
                VerticesNormals[0, z] = (normal1 + normal2 + normal3 + normal4) / 4.0f;
                VerticesNormals[0, z].Normalize();

            }
            //Right
            for (int z = 1; z < heightMap.Height - 1; z++)
            {
                Vector3 currentPosition = vertices[(heightMap.Width - 1) * heightMap.Height + z].Position;
                Vector3 normal1 = Vector3.Cross(vertices[(heightMap.Width - 1) * heightMap.Height + (z - 1)].Position - currentPosition, vertices[(heightMap.Width - 2) * heightMap.Height + (z - 1)].Position - currentPosition);
                Vector3 normal2 = Vector3.Cross(vertices[(heightMap.Width - 2) * heightMap.Height + (z - 1)].Position - currentPosition, vertices[(heightMap.Width - 2) * heightMap.Height + z].Position - currentPosition);
                Vector3 normal3 = Vector3.Cross(vertices[(heightMap.Width - 2) * heightMap.Height + z].Position - currentPosition, vertices[(heightMap.Width - 2) * heightMap.Height + (z + 1)].Position - currentPosition);
                Vector3 normal4 = Vector3.Cross(vertices[(heightMap.Width - 2) * heightMap.Height + (z + 1)].Position - currentPosition, vertices[(heightMap.Width - 1) * heightMap.Height + (z + 1)].Position - currentPosition);
                normal1.Normalize();
                normal2.Normalize();
                normal3.Normalize();
                normal4.Normalize();
                vertices[(heightMap.Width - 1) * heightMap.Height + z].Normal = (normal1 + normal2 + normal3 + normal4) / 4.0f;
                VerticesNormals[heightMap.Width - 1, z] = (normal1 + normal2 + normal3 + normal4) / 4.0f;
                VerticesNormals[heightMap.Width - 1, z].Normalize();

            }

            //Top-Left;
            {
                Vector3 currentPosition = vertices[0].Position;
                Vector3 normal1 = Vector3.Cross(vertices[1].Position - currentPosition, vertices[heightMap.Height + 1].Position - currentPosition);
                Vector3 normal2 = Vector3.Cross(vertices[heightMap.Height + 1].Position - currentPosition, vertices[heightMap.Height].Position - currentPosition);
                normal1.Normalize();
                normal2.Normalize();
                vertices[0].Normal = (normal1 + normal2) / 2.0f;
                VerticesNormals[0, 0] = (normal1 + normal2) / 2.0f;
                VerticesNormals[0, 0].Normalize();

            }
            //Top-Right;
            {
                Vector3 currentPosition = vertices[(heightMap.Width - 1) * heightMap.Height].Position;
                Vector3 normal1 = Vector3.Cross(vertices[(heightMap.Width - 2) * heightMap.Height].Position - currentPosition, vertices[(heightMap.Width - 2) * heightMap.Height + 1].Position - currentPosition);
                Vector3 normal2 = Vector3.Cross(vertices[(heightMap.Width - 2) * heightMap.Height + 1].Position - currentPosition, vertices[(heightMap.Width - 1) * heightMap.Height + 1].Position - currentPosition);
                normal1.Normalize();
                normal2.Normalize();
                vertices[(heightMap.Width - 1) * heightMap.Height].Normal = (normal1 + normal2) / 2.0f;
                VerticesNormals[heightMap.Width - 1, 0] = (normal1 + normal2) / 2.0f;
                VerticesNormals[heightMap.Width - 1, 0].Normalize();

            }

            //Bottom-Left;
            {
                Vector3 currentPosition = vertices[heightMap.Height - 1].Position;
                Vector3 normal1 = Vector3.Cross(vertices[heightMap.Height * 2 - 1].Position - currentPosition, vertices[heightMap.Height * 2 - 2].Position - currentPosition);
                Vector3 normal2 = Vector3.Cross(vertices[heightMap.Height * 2 - 2].Position - currentPosition, vertices[heightMap.Height - 2].Position - currentPosition);
                normal1.Normalize();
                normal2.Normalize();
                vertices[heightMap.Height - 1].Normal = (normal1 + normal2) / 2.0f;
                VerticesNormals[0, heightMap.Height - 1] = (normal1 + normal2) / 2.0f;
                VerticesNormals[0, heightMap.Height - 1].Normalize();

            }

            //Bottom-Right;
            {
                Vector3 currentPosition = vertices[heightMap.Width * heightMap.Height - 1].Position;
                Vector3 normal1 = Vector3.Cross(vertices[heightMap.Width * heightMap.Height - 2].Position - currentPosition, vertices[(heightMap.Width - 1) * heightMap.Height - 2].Position - currentPosition);
                Vector3 normal2 = Vector3.Cross(vertices[(heightMap.Width - 1) * heightMap.Height - 2].Position - currentPosition, vertices[(heightMap.Width - 1) * heightMap.Height - 1].Position - currentPosition);
                normal1.Normalize();
                normal2.Normalize();
                vertices[heightMap.Width * heightMap.Height - 1].Normal = (normal1 + normal2) / 2.0f;
                VerticesNormals[heightMap.Width - 1, heightMap.Height - 1] = (normal1 + normal2) / 2.0f;
                VerticesNormals[heightMap.Width - 1, heightMap.Height - 1].Normalize();

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

        //--------------------Update&Draw--------------------//

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