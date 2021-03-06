﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TankProject
{
    class Enemy : Boid

    {
        private static Model model;
        private float modelScale;
        private const float TANK_HEIGHT_FROM_FLOOR = 0.01f;
        internal Vector3 lastFramePosition;
        internal Matrix[] boneTransformations;
        internal OBB boundingBox;

        internal Enemy(Vector3 position, Vector3 rotation, float modelScale, GameStage game) : base(position, rotation, game)
        {
            this.relativeForward = this.Forward = Vector3.Forward;
            this.relativeRight = this.Right = Vector3.Right;
            this.lastFramePosition = position;
            this.modelScale = modelScale;
            boneTransformations = new Matrix[model.Bones.Count];

            this.boundingBox = OBB.CreateFromSphereForEnemies(model.Meshes[0].BoundingSphere, position, 0.04f, base.rotationMatrix); ;
        }

        internal static void Load(ContentManager content, Material material, Light light)
        {
            model = content.Load<Model>("body");

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
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
        }

        //Corrections
        private void HeightFollow()
        {
            Vector2 positionXZ = new Vector2(position.X, position.Z);
            Vector2 roundedPositionXZ = new Vector2((int)this.position.X, (int)this.position.Z);

            if(position.X > 0 && position.X < Floor.heightMap.Width - 1 && position.Z > 0 && position.Z < Floor.heightMap.Height - 1)
            {
                this.position.Y = TANK_HEIGHT_FROM_FLOOR + Interpolation.BiLinear(positionXZ, roundedPositionXZ, 1.0f,
            Floor.VerticesHeight[(int)positionXZ.X, (int)positionXZ.Y], Floor.VerticesHeight[(int)positionXZ.X + 1, (int)positionXZ.Y],
            Floor.VerticesHeight[(int)positionXZ.X, (int)positionXZ.Y + 1], Floor.VerticesHeight[(int)positionXZ.X + 1, (int)positionXZ.Y + 1]);
            }
            else
            {
                this.position.Y = 1;
            }

        }
        private void NormalFollow()
        {
            Vector2 positionXZ = new Vector2(position.X, position.Z);
            Vector2 roundedPositionXZ = new Vector2((int)this.position.X, (int)this.position.Z);

            if(position.X > 0 && position.X < Floor.heightMap.Width - 1 && position.Z > 0 && position.Z < Floor.heightMap.Height - 1)
            {
                this.Up = Interpolation.BiLinear(positionXZ, roundedPositionXZ, 1.0f,
                Floor.VerticesNormals[(int)positionXZ.X, (int)positionXZ.Y], Floor.VerticesNormals[(int)positionXZ.X + 1, (int)positionXZ.Y],
                Floor.VerticesNormals[(int)positionXZ.X, (int)positionXZ.Y + 1], Floor.VerticesNormals[(int)positionXZ.X + 1, (int)positionXZ.Y + 1]);
            }
            else
            {
                this.Up = Vector3.Up;
            }

            this.Up.Normalize();
            this.Forward = Vector3.Cross(Up, relativeRight);
            this.Forward.Normalize();
            this.Right = Vector3.Cross(relativeForward, Up);
            this.Right.Normalize();
        }


        internal override void Update(Vector3 targetPosition, GameTime deltaTime)
        {
            lastFramePosition = position;
            base.Update(targetPosition, deltaTime);

            HeightFollow();
            NormalFollow();

            rotationMatrix.Up = this.Up;
            //The 3d model is facing backwards.
            rotationMatrix.Forward = -this.Forward;
            rotationMatrix.Right = -this.Right;

            model.Root.Transform = Matrix.CreateScale(modelScale) * rotationMatrix * Matrix.CreateTranslation(position);
            model.CopyAbsoluteBoneTransformsTo(boneTransformations);
            this.boundingBox.Update(this.position, rotationMatrix.Forward, rotationMatrix.Right, rotationMatrix.Up);
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
