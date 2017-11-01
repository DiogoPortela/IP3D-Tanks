using Microsoft.Xna.Framework;
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
        private Model tankModel;
        private ModelBone turretBone, cannonBone, hatchBone, rightSteerBone, leftSteerBone, rightFrontWheelBone, leftFrontWheelBone, rightBackWheelBone, leftBackWheelBone;
        private Matrix turretTransform, cannonTransform, hatchTransform, rightSteerTransform, leftSteerTransform, rightFrontWheelTransform, leftFrontWheelTransform, rightBackWheelTransform, leftBackWheelTransform;

        private float modelScale;
        private float turretAngle = 0, cannonAngle = 0, hatchetAngle = 0, rightFrontWheelAngle = 0, leftFrontWheelAngle = 0, rightBackWheelAngle = 0, leftBackWheelAngle = 0;

        internal Matrix[] boneTransformations;
        


        internal Player(Vector3 position, Vector3 rotation, Vector3 velocity, float modelScale)
            : base(position, rotation, velocity)
        {
            this.modelScale = modelScale;
            
        }
        
        internal void Move(GameTime gameTime)
        {
            //movimento tank
            if (Input.IsPressedDown(Keys.W) && !Input.IsPressedDown(Keys.S))
            {
                this.position += this.relativeForward * this.velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.rightFrontWheelAngle += MathHelper.ToRadians(1f);
                this.leftFrontWheelAngle += MathHelper.ToRadians(1f);
            }
            if (Input.IsPressedDown(Keys.S) && !Input.IsPressedDown(Keys.W))
            {
                this.position -= this.relativeForward * this.velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.rightFrontWheelAngle -= MathHelper.ToRadians(1f);
                this.leftFrontWheelAngle -= MathHelper.ToRadians(1f);
            }
            if (Input.IsPressedDown(Keys.A) && !Input.IsPressedDown(Keys.D))
            {
                this.position += this.relativeRight * this.velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (Input.IsPressedDown(Keys.D) && !Input.IsPressedDown(Keys.A))
            {
                this.position -= this.relativeRight * this.velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            //movimento torre
            if (Input.IsPressedDown(Keys.Up) && !Input.IsPressedDown(Keys.Down))
                if(this.cannonAngle < Math.PI / 4)
                    this.cannonAngle -= MathHelper.ToRadians(1f);
            if (Input.IsPressedDown(Keys.Down) && !Input.IsPressedDown(Keys.Up))
                if(this.cannonAngle > 0)
                    this.cannonAngle += MathHelper.ToRadians(1f);
            if (Input.IsPressedDown(Keys.Left) && !Input.IsPressedDown(Keys.Right))
                this.turretAngle -= MathHelper.ToRadians(1f);
            if (Input.IsPressedDown(Keys.Right) && !Input.IsPressedDown(Keys.Left))
                this.turretAngle += MathHelper.ToRadians(1f);

            //abrir hatchet
            if(Input.WasPressed(Keys.O)) //TODO: meter o hatchet a abrir devagar
            {
                if(hatchetAngle < Math.PI / 8f)
                {
                    while (hatchetAngle < Math.PI / 2f)
                        this.hatchetAngle += MathHelper.ToRadians(1f);
                }
                else
                    while (hatchetAngle > 0)
                        this.hatchetAngle -= MathHelper.ToRadians(1f);
            }

        }

        internal void LoadModelBones(ContentManager content)
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
            this.rightBackWheelTransform = rightFrontWheelBone.Transform;

            this.leftBackWheelBone = tankModel.Bones["l_back_wheel_geo"];
            this.leftBackWheelTransform = rightFrontWheelBone.Transform;

            this.boneTransformations = new Matrix[tankModel.Bones.Count];
        }

        internal void Update(GameTime gameTime)
        {
            tankModel.Root.Transform = Matrix.CreateScale(modelScale);
            turretBone.Transform = Matrix.CreateRotationY(turretAngle) * turretTransform;
            cannonBone.Transform = Matrix.CreateRotationX(cannonAngle) * cannonTransform;
            rightFrontWheelBone.Transform = Matrix.CreateRotationX(rightFrontWheelAngle) * rightFrontWheelTransform;
            leftFrontWheelBone.Transform = Matrix.CreateRotationX(leftFrontWheelAngle) * leftFrontWheelTransform;
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

    }
}
