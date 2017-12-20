using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TankProject
{
    /// <summary>
    ///Floor class. Generates a floor.
    /// </summary>
    static class Floor
    {
        private static VertexBuffer vertexBuffer;
        private static IndexBuffer indexBuffer;
        private static BasicEffect effect;
        internal static Texture2D heightMap;
        private static Texture2D texture;
        internal static float[,] VerticesHeight;
        internal static Vector3[,] VerticesNormals;

        //--------------------Functions--------------------//

        internal static void Start(ContentManager content, Camera camera, Material material, Light light)
        {
            heightMap = content.Load<Texture2D>("lh3d1");
            texture = content.Load<Texture2D>("sand");

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

                    VerticesNormals[x, z] = vertices[x * heightMap.Height + z].Normal = Vector3.Normalize((normal1 + normal2 + normal3 + normal4 + normal5 + normal6 + normal7 + normal8) / 8.0f);

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

                VerticesNormals[x, 0] = vertices[x * heightMap.Height].Normal = Vector3.Normalize((normal1 + normal2 + normal3 + normal4) / 4.0f);

            }
            // Bottom
            for (int x = 1; x < heightMap.Width - 1; x++)
            {
                Vector3 currentPosition = vertices[x * heightMap.Height + (heightMap.Height - 1)].Position;
                Vector3 normal1 = Vector3.Cross(vertices[(x + 1) * heightMap.Height + (heightMap.Height - 1)].Position - currentPosition, vertices[(x + 1) * heightMap.Height + (heightMap.Height - 2)].Position - currentPosition);
                Vector3 normal2 = Vector3.Cross(vertices[(x + 1) * heightMap.Height + (heightMap.Height - 2)].Position - currentPosition, vertices[x * heightMap.Height + (heightMap.Height - 2)].Position - currentPosition);
                Vector3 normal3 = Vector3.Cross(vertices[x * heightMap.Height + (heightMap.Height - 2)].Position - currentPosition, vertices[(x - 1) * heightMap.Height + (heightMap.Height - 2)].Position - currentPosition);
                Vector3 normal4 = Vector3.Cross(vertices[(x - 1) * heightMap.Height + (heightMap.Height - 2)].Position - currentPosition, vertices[(x - 1) * heightMap.Height + (heightMap.Height - 1)].Position - currentPosition);

                VerticesNormals[x, heightMap.Height - 1] = vertices[x * heightMap.Height + (heightMap.Height - 1)].Normal = Vector3.Normalize((normal1 + normal2 + normal3 + normal4) / 4.0f);

            }
            // Left
            for (int z = 1; z < heightMap.Height - 1; z++)
            {
                Vector3 currentPosition = vertices[z].Position;
                Vector3 normal1 = Vector3.Cross(vertices[z + 1].Position - currentPosition, vertices[heightMap.Height + (z + 1)].Position - currentPosition);
                Vector3 normal2 = Vector3.Cross(vertices[heightMap.Height + (z + 1)].Position - currentPosition, vertices[heightMap.Height + z].Position - currentPosition);
                Vector3 normal3 = Vector3.Cross(vertices[heightMap.Height + z].Position - currentPosition, vertices[heightMap.Height + (z - 1)].Position - currentPosition);
                Vector3 normal4 = Vector3.Cross(vertices[heightMap.Height + (z - 1)].Position - currentPosition, vertices[z - 1].Position - currentPosition);

                VerticesNormals[0, z] = vertices[z].Normal = Vector3.Normalize((normal1 + normal2 + normal3 + normal4) / 4.0f);

            }
            //Right
            for (int z = 1; z < heightMap.Height - 1; z++)
            {
                Vector3 currentPosition = vertices[(heightMap.Width - 1) * heightMap.Height + z].Position;
                Vector3 normal1 = Vector3.Cross(vertices[(heightMap.Width - 1) * heightMap.Height + (z - 1)].Position - currentPosition, vertices[(heightMap.Width - 2) * heightMap.Height + (z - 1)].Position - currentPosition);
                Vector3 normal2 = Vector3.Cross(vertices[(heightMap.Width - 2) * heightMap.Height + (z - 1)].Position - currentPosition, vertices[(heightMap.Width - 2) * heightMap.Height + z].Position - currentPosition);
                Vector3 normal3 = Vector3.Cross(vertices[(heightMap.Width - 2) * heightMap.Height + z].Position - currentPosition, vertices[(heightMap.Width - 2) * heightMap.Height + (z + 1)].Position - currentPosition);
                Vector3 normal4 = Vector3.Cross(vertices[(heightMap.Width - 2) * heightMap.Height + (z + 1)].Position - currentPosition, vertices[(heightMap.Width - 1) * heightMap.Height + (z + 1)].Position - currentPosition);

                VerticesNormals[heightMap.Width - 1, z] = vertices[(heightMap.Width - 1) * heightMap.Height + z].Normal = Vector3.Normalize((normal1 + normal2 + normal3 + normal4) / 4.0f);

            }

            //Top-Left;
            {
                Vector3 currentPosition = vertices[0].Position;
                Vector3 normal1 = Vector3.Cross(vertices[1].Position - currentPosition, vertices[heightMap.Height + 1].Position - currentPosition);
                Vector3 normal2 = Vector3.Cross(vertices[heightMap.Height + 1].Position - currentPosition, vertices[heightMap.Height].Position - currentPosition);

                VerticesNormals[0, 0] = vertices[0].Normal = Vector3.Normalize((normal1 + normal2) / 2.0f);

            }
            //Top-Right;
            {
                Vector3 currentPosition = vertices[(heightMap.Width - 1) * heightMap.Height].Position;
                Vector3 normal1 = Vector3.Cross(vertices[(heightMap.Width - 2) * heightMap.Height].Position - currentPosition, vertices[(heightMap.Width - 2) * heightMap.Height + 1].Position - currentPosition);
                Vector3 normal2 = Vector3.Cross(vertices[(heightMap.Width - 2) * heightMap.Height + 1].Position - currentPosition, vertices[(heightMap.Width - 1) * heightMap.Height + 1].Position - currentPosition);

                VerticesNormals[heightMap.Width - 1, 0] = vertices[(heightMap.Width - 1) * heightMap.Height].Normal = Vector3.Normalize((normal1 + normal2) / 2.0f);

            }

            //Bottom-Left;
            {
                Vector3 currentPosition = vertices[heightMap.Height - 1].Position;
                Vector3 normal1 = Vector3.Cross(vertices[heightMap.Height * 2 - 1].Position - currentPosition, vertices[heightMap.Height * 2 - 2].Position - currentPosition);
                Vector3 normal2 = Vector3.Cross(vertices[heightMap.Height * 2 - 2].Position - currentPosition, vertices[heightMap.Height - 2].Position - currentPosition);

                VerticesNormals[0, heightMap.Height - 1] = vertices[heightMap.Height - 1].Normal = Vector3.Normalize((normal1 + normal2) / 2.0f);

            }

            //Bottom-Right;
            {
                Vector3 currentPosition = vertices[heightMap.Width * heightMap.Height - 1].Position;
                Vector3 normal1 = Vector3.Cross(vertices[heightMap.Width * heightMap.Height - 2].Position - currentPosition, vertices[(heightMap.Width - 1) * heightMap.Height - 2].Position - currentPosition);
                Vector3 normal2 = Vector3.Cross(vertices[(heightMap.Width - 1) * heightMap.Height - 2].Position - currentPosition, vertices[(heightMap.Width - 1) * heightMap.Height - 1].Position - currentPosition);

                VerticesNormals[heightMap.Width - 1, heightMap.Height - 1] = vertices[heightMap.Width * heightMap.Height - 1].Normal = Vector3.Normalize((normal1 + normal2) / 2.0f);

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

        internal static float GetHeight(Vector3 objectPosition)
        {
            Vector2 positionXZ = new Vector2(objectPosition.X, objectPosition.Z);
            Vector2 roundedPositionXZ = new Vector2((int)objectPosition.X, (int)objectPosition.Z);

            return Interpolation.BiLinear(positionXZ, roundedPositionXZ, 1.0f,
            VerticesHeight[(int)positionXZ.X, (int)positionXZ.Y], VerticesHeight[(int)positionXZ.X + 1, (int)positionXZ.Y],
            VerticesHeight[(int)positionXZ.X, (int)positionXZ.Y + 1], VerticesHeight[(int)positionXZ.X + 1, (int)positionXZ.Y + 1]);
        }
    }
}