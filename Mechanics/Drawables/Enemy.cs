using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TankProject
{
    class Enemy : Boid
    {
        private static Model model;
        private const float TANK_HEIGHT_FROM_FLOOR = 0.01f;

        internal Matrix[] boneTransformations;


        internal Enemy(Vector3 position, Vector3 rotation) : base(position, rotation)
        {
            this.relativeForward = this.Forward = Vector3.Forward;
            this.relativeRight = this.Right = Vector3.Right;
            boneTransformations = new Matrix[model.Bones.Count];
        }

        internal static void Load(ContentManager content)
        {
            model = content.Load<Model>("tank");
        }

        //Corrections
        private void HeightFollow()
        {
            Vector2 positionXZ = new Vector2(position.X, position.Z);
            Vector2 roundedPositionXZ = new Vector2((int)this.position.X, (int)this.position.Z);

            this.position.Y = TANK_HEIGHT_FROM_FLOOR + Interpolation.BiLinear(positionXZ, roundedPositionXZ, 1.0f,
            Floor.VerticesHeight[(int)positionXZ.X, (int)positionXZ.Y], Floor.VerticesHeight[(int)positionXZ.X + 1, (int)positionXZ.Y],
            Floor.VerticesHeight[(int)positionXZ.X, (int)positionXZ.Y + 1], Floor.VerticesHeight[(int)positionXZ.X + 1, (int)positionXZ.Y + 1]);
        }
        private void NormalFollow()
        {
            Vector2 positionXZ = new Vector2(position.X, position.Z);
            Vector2 roundedPositionXZ = new Vector2((int)this.position.X, (int)this.position.Z);

            this.Up = Interpolation.BiLinear(positionXZ, roundedPositionXZ, 1.0f,
                Floor.VerticesNormals[(int)positionXZ.X, (int)positionXZ.Y], Floor.VerticesNormals[(int)positionXZ.X + 1, (int)positionXZ.Y],
                Floor.VerticesNormals[(int)positionXZ.X, (int)positionXZ.Y + 1], Floor.VerticesNormals[(int)positionXZ.X + 1, (int)positionXZ.Y + 1]);

            this.Up.Normalize();
            this.Forward = Vector3.Cross(Up, relativeRight);
            this.Forward.Normalize();
            this.Right = Vector3.Cross(relativeForward, Up);
            this.Right.Normalize();
        }


        internal override void Update(Vector3 targetPosition, GameTime deltaTime)
        {
            base.Update(targetPosition, deltaTime);

            HeightFollow();
            NormalFollow();

            rotationMatrix.Up = this.Up;
            //The 3d model is facing backwards.
            rotationMatrix.Forward = -this.Forward;
            rotationMatrix.Right = -this.Right;

            model.Root.Transform = Matrix.CreateScale(0.0005f) * rotationMatrix * Matrix.CreateTranslation(position);
            model.CopyAbsoluteBoneTransformsTo(boneTransformations);

        }

        internal void Draw(GraphicsDevice device, Camera cam)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = boneTransformations[mesh.ParentBone.Index];
                    effect.View = cam.ViewMatrix;
                    effect.Projection = cam.ProjectionMatrix;
                }
                mesh.Draw();
            }
        }

    }
}
