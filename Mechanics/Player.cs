﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TankProject
{
    class Player : GameObject
    {
        private Vector3 relativeForward, relativeRight;
        private Vector3 turretRotationVelocity;
        private Vector3 Up, Forward, Right;
        private Model tankModel;
        private ModelBone turretBone, cannonBone, hatchBone, rightSteerBone, leftSteerBone, rightFrontWheelBone, leftFrontWheelBone, rightBackWheelBone, leftBackWheelBone;
        private Matrix turretTransform, cannonTransform, hatchTransform, rightSteerTransform, leftSteerTransform, rightFrontWheelTransform, leftFrontWheelTransform, rightBackWheelTransform, leftBackWheelTransform;

        private float modelScale;
        private float turretAngle = 0, cannonAngle = 0, hatchetAngle = 0, rightFrontWheelAngle = 0, leftFrontWheelAngle = 0, rightBackWheelAngle = 0, leftBackWheelAngle = 0;

        internal Matrix[] boneTransformations;

        internal Player(Vector3 position, Vector3 rotation, Vector3 velocity, float modelScale)
            : base(position, rotation, velocity)
        {
            this.relativeForward = Vector3.Forward;
            this.relativeRight = Vector3.Right;
            this.Up = Vector3.Up;
            this.Forward = Vector3.Forward;
            this.Right = Vector3.Right;
            this.modelScale = modelScale;


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        private void Move(GameTime gameTime)
        {
            if (Input.IsPressedDown(Keys.W) && !Input.IsPressedDown(Keys.S))
            {
                this.position -= this.relativeForward * (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.rightFrontWheelAngle += MathHelper.ToRadians(10f);
                this.leftFrontWheelAngle += MathHelper.ToRadians(10f);
                this.rightBackWheelAngle += MathHelper.ToRadians(10f);
                this.leftBackWheelAngle += MathHelper.ToRadians(10f);
            }
            else if (Input.IsPressedDown(Keys.S) && !Input.IsPressedDown(Keys.W))
            {
                this.position += this.relativeForward * (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.rightFrontWheelAngle -= MathHelper.ToRadians(10f);
                this.leftFrontWheelAngle -= MathHelper.ToRadians(10f);
                this.rightBackWheelAngle -= MathHelper.ToRadians(10f);
                this.leftBackWheelAngle -= MathHelper.ToRadians(10f);
            }
        }

        private void Rotate(GameTime gameTime)
        {
            if (Input.IsPressedDown(Keys.A) && !Input.IsPressedDown(Keys.D))
            {
                rotation.X += MathHelper.ToRadians(1f);
            }
            else if (Input.IsPressedDown(Keys.D) && !Input.IsPressedDown(Keys.A))
            {
                rotation.X -= MathHelper.ToRadians(1f);
            }

            //movimento torre
            if (Input.IsPressedDown(Keys.Up) && !Input.IsPressedDown(Keys.Down))
                if (this.cannonAngle < Math.PI / 4)
                    this.cannonAngle -= MathHelper.ToRadians(1f);
            else if (Input.IsPressedDown(Keys.Down) && !Input.IsPressedDown(Keys.Up))
                if (this.cannonAngle > 0)
                    this.cannonAngle += MathHelper.ToRadians(1f);
            //TODO: FIX CANNON.
            if (Input.IsPressedDown(Keys.Left) && !Input.IsPressedDown(Keys.Right))
                this.turretAngle -= MathHelper.ToRadians(1f);
            else if (Input.IsPressedDown(Keys.Right) && !Input.IsPressedDown(Keys.Left))
                this.turretAngle += MathHelper.ToRadians(1f);
        }

        private void UpdateHatchet(GameTime gameTime)
        {
            //abrir hatchet
            if (Input.IsPressedDown(Keys.O)) //TODO: meter o hatchet a abrir devagar
            {
                if (hatchetAngle < Math.PI / 8f)
                {
                    this.hatchetAngle += MathHelper.ToRadians(100f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else if (hatchetAngle > 0)
                    this.hatchetAngle -= MathHelper.ToRadians(100f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        internal void LoadModelBones(ContentManager content, Material material, Light light)
        {
            this.tankModel = content.Load<Model>("tank");

            this.cannonBone = tankModel.Bones["canon_geo"];
            this.cannonTransform = cannonBone.Transform;

            this.turretBone = tankModel.Bones["turret_geo"];
            this.turretTransform = turretBone.Transform;

            this.hatchBone = tankModel.Bones["hatch_geo"];
            this.hatchTransform = hatchBone.Transform;

            this.rightSteerBone = tankModel.Bones["r_steer_geo"];
            this.rightSteerTransform = rightSteerBone.Transform;

            this.leftSteerBone = tankModel.Bones["l_steer_geo"];
            this.leftSteerTransform = leftSteerBone.Transform;

            this.rightFrontWheelBone = tankModel.Bones["r_front_wheel_geo"];
            this.rightFrontWheelTransform = rightFrontWheelBone.Transform;

            this.leftFrontWheelBone = tankModel.Bones["l_front_wheel_geo"];
            this.leftFrontWheelTransform = leftFrontWheelBone.Transform;

            this.rightBackWheelBone = tankModel.Bones["r_back_wheel_geo"];
            this.rightBackWheelTransform = rightBackWheelBone.Transform;

            this.leftBackWheelBone = tankModel.Bones["l_back_wheel_geo"];
            this.leftBackWheelTransform = leftBackWheelBone.Transform;

            this.boneTransformations = new Matrix[tankModel.Bones.Count];

            foreach (ModelMesh mesh in tankModel.Meshes)
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

        internal void Update(GameTime gameTime)
        {
            Move(gameTime);
            Rotate(gameTime);

            relativeForward = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationY(rotation.X));
            relativeForward.Normalize();
            relativeRight = Vector3.Transform(Vector3.Right, Matrix.CreateRotationY(rotation.X));
            relativeRight.Normalize();

            UpdateHatchet(gameTime);
            HeightFollow();
            NormalFollow();

            transformMatrix = Matrix.CreateTranslation(position);;
            //rotationMatrix = Matrix.Invert(Matrix.CreateLookAt(Vector3.Zero, this.Forward, this.Up));
            rotationMatrix.Up = this.Up;
            rotationMatrix.Forward = this.Forward;
            rotationMatrix.Right = this.Right;

            System.Diagnostics.Debug.WriteLine("Scale:" + rotationMatrix.Scale);
            System.Diagnostics.Debug.WriteLine("Rotation:" + rotationMatrix.Rotation);

            tankModel.Root.Transform = Matrix.CreateScale(modelScale) * rotationMatrix * transformMatrix;
            turretBone.Transform = Matrix.CreateRotationY(turretAngle) * turretTransform;
            cannonBone.Transform = Matrix.CreateRotationX(cannonAngle) * cannonTransform;
            rightFrontWheelBone.Transform = Matrix.CreateRotationX(rightFrontWheelAngle) * rightFrontWheelTransform;
            leftFrontWheelBone.Transform = Matrix.CreateRotationX(leftFrontWheelAngle) * leftFrontWheelTransform;
            rightBackWheelBone.Transform = Matrix.CreateRotationX(rightBackWheelAngle) * rightBackWheelTransform;
            leftBackWheelBone.Transform = Matrix.CreateRotationX(leftBackWheelAngle) * leftBackWheelTransform;
            hatchBone.Transform = Matrix.CreateRotationX(hatchetAngle) * hatchTransform;

            tankModel.CopyAbsoluteBoneTransformsTo(boneTransformations);
        }

        internal void Draw(Camera cam)
        {
            foreach (ModelMesh mesh in tankModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = boneTransformations[mesh.ParentBone.Index];
                    effect.View = cam.ViewMatrix;
                    effect.Projection = cam.ProjectionMatrix;
                    effect.EnableDefaultLighting();
                }
                mesh.Draw();
            }
        }

        private void HeightFollow()
        {
            Vector2 positionXZ = new Vector2(position.X, position.Z);
            Vector2 roundedPositionXZ = new Vector2((int)this.position.X, (int)this.position.Z);

            this.position.Y = 0.001f /*offset*/ + Interpolation.BiLinear(positionXZ, roundedPositionXZ, 1.0f,
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
    }
}
