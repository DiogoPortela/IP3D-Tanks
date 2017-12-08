using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TankProject
{
    static class Skybox
    {
        private static Model skyboxMesh;
        private static Texture2D skyboxTexture;

        internal static void Load(ContentManager content)
        {
            skyboxMesh = content.Load<Model>("SkyboxMesh");
            skyboxTexture = content.Load<Texture2D>("SkyboxTexture");

            foreach (BasicEffect effect in skyboxMesh.Meshes[0].Effects)
            {
                effect.TextureEnabled = true;
                effect.Texture = skyboxTexture;
                effect.World = Matrix.Identity;
            }
        }
        internal static void Draw(GraphicsDevice device, Camera cam)
        {
            foreach (BasicEffect effect in skyboxMesh.Meshes[0].Effects)
            {
                effect.Texture = skyboxTexture;
                effect.World = Matrix.Identity;
                effect.View = cam.ViewMatrix;
                effect.Projection = cam.ProjectionMatrix;
            }
            skyboxMesh.Meshes[0].Draw();
        }
    }
}
