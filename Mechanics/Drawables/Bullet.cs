using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TankProject
{
    class Bullet : GameObject
    {
        private static float G = -9.8f;
        private static float SHOOTING_POWER = 1f;
        private static Model bulletModel;

        public Bullet(Vector3 startingPosition, Vector3 velocity, Vector3 normal) :
            base(startingPosition, Vector3.Zero, velocity)
        {
            this.position = startingPosition;
            this.velocity = velocity * SHOOTING_POWER;

            Forward = Vector3.Normalize(velocity);
            Up = normal;
            Right = Vector3.Cross(Up, Forward);

            this.rotationMatrix.Forward = Forward;
            this.rotationMatrix.Up = Up;
            this.rotationMatrix.Right = Right;
        }

        public static void LoadModel(ContentManager content)
        {
            bulletModel = content.Load<Model>("TankBullet");
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            this.velocity.Y += G * deltaTime;
            this.position += this.velocity * deltaTime;
        }

        internal void Draw(Camera cam)
        {
            foreach (ModelMesh mesh in bulletModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = Matrix.CreateScale(0.5f) * rotationMatrix * Matrix.CreateTranslation(position);
                    effect.View = cam.ViewMatrix;
                    effect.Projection = cam.ProjectionMatrix;
                }
                mesh.Draw();
            }
        }
    }
}
