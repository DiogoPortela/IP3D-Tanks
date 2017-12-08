using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TankProject
{
    class Bullet : GameObject
    {
        //Statics
        public static float BULLET_SCALE = 0.25f;
        private static float G = -9.8f;
        private static float SHOOTING_POWER = 15f;
        private static Model bulletModel;
        private static Texture2D bulletTexture;
        private static float mass = 1.5f;

        //Individual
        internal OBB boundingBox;

        //--------------------Constructors--------------------//
        public Bullet(Vector3 startingPosition, Vector3 velocity, Vector3 normal) :
            base(startingPosition, Vector3.Zero, velocity)
        {
            this.position = startingPosition;
            this.velocity = velocity * SHOOTING_POWER;

            Forward = -Vector3.Normalize(velocity);
            Up = normal;
            Right = Vector3.Cross(Up, Forward);

            this.rotationMatrix.Forward = Forward;
            this.rotationMatrix.Up = Up;
            this.rotationMatrix.Right = Right;

            boundingBox = OBB.CreateFromSphere(bulletModel.Meshes[0].BoundingSphere, startingPosition, BULLET_SCALE, rotationMatrix);
        }

        //--------------------Functions--------------------//
        public static void LoadModel(ContentManager content, Material material, Light light)
        {
            bulletModel = content.Load<Model>("TankBullet");
            bulletTexture = content.Load<Texture2D>("BulletTexture");

            foreach (BasicEffect effect in bulletModel.Meshes[0].Effects)
            {
                effect.TextureEnabled = true;
                effect.Texture = bulletTexture;

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
            }
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            this.velocity.Y += G * mass * deltaTime;
            this.position += this.velocity * deltaTime;
            this.rotationMatrix.Forward = -Vector3.Normalize(this.velocity);

            this.boundingBox.Update(position, rotationMatrix.Forward, rotationMatrix.Right, rotationMatrix.Up);

        }
        internal void Draw(Camera cam)
        {
            foreach (BasicEffect effect in bulletModel.Meshes[0].Effects)
            {
                effect.World = Matrix.CreateScale(BULLET_SCALE) * rotationMatrix * Matrix.CreateTranslation(position);
                effect.View = cam.ViewMatrix;
                effect.Projection = cam.ProjectionMatrix;
            }
            bulletModel.Meshes[0].Draw();
        }
    }
}
