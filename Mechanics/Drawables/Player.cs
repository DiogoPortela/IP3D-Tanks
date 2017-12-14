using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace TankProject
{
    /// <summary>
    /// Player class.
    /// </summary>
    class Player : GameObject
    {
        private const float TANK_HEIGHT_FROM_FLOOR = 0.001f;

        //Model information
        private static Model tankModel;
        private ModelBone turretBone, cannonBone, hatchBone, rightSteerBone, leftSteerBone, rightFrontWheelBone, leftFrontWheelBone, rightBackWheelBone, leftBackWheelBone;
        private Matrix hatchTransform, rightSteerTransform, leftSteerTransform, rightFrontWheelTransform, leftFrontWheelTransform, rightBackWheelTransform, leftBackWheelTransform;

        internal Bone turret, cannon;

        private float modelScale;
        private float hatchetAngle = 0, rightSteerAngle = 0, leftSteerAngle = 0, rightFrontWheelAngle = 0, leftFrontWheelAngle = 0, rightBackWheelAngle = 0, leftBackWheelAngle = 0;

        private bool isOpenning; //used to slowly open hatchet

        internal Matrix[] boneTransformations;
        internal OBB boundingBox;

        internal List<Bullet> bulletList;

        //Player information

        internal enum PlayerNumber { PlayerOne = 1, PlayerTwo };
        private PlayerNumber playerNumber;

        private PlayerKeys playerKeys;
    
        private static GameStage gameState;

        //test zone
        Vector3 aceleration;
        Vector3 velocity;


        //--------------------Constructors--------------------//

        internal Player(Vector3 position, Vector3 rotation, Vector3 velocity, float modelScale, PlayerNumber number, GameStage currentState)
            : base(position, rotation, velocity)
        {
            this.relativeForward = this.Forward = Vector3.Forward;
            this.relativeRight = this.Right = Vector3.Right;
            this.Up = Vector3.Up;
            this.modelScale = modelScale;
            this.playerNumber = number;
            bulletList = new List<Bullet>();
            //boundingBoxes = new List<BoundingBox>();
            SetPlayerKeys();
            gameState = currentState;   //TODO: CLEAN
            aceleration = Vector3.Zero;
            velocity = Vector3.Zero;
        }

        //--------------------Functions--------------------//

        //Loads
        internal void LoadModelBones(ContentManager content, Material material, Light light)
        {
            tankModel = content.Load<Model>("tank");
            boundingBox = OBB.CreateFromSphere(tankModel.Root.Meshes[0].BoundingSphere, this.position, modelScale, this.rotationMatrix);

            this.cannonBone = tankModel.Bones["canon_geo"];
            cannon = new Bone(cannonBone.Transform, this.position, Vector3.Zero, modelScale);
            cannon.boundingBox = OBB.CreateFromSphere(cannonBone.Meshes[0].BoundingSphere, cannon.position, modelScale, this.rotationMatrix);

            this.turretBone = tankModel.Bones["turret_geo"];
            turret = new Bone(turretBone.Transform, this.position, Vector3.Zero, modelScale);
            turret.boundingBox = OBB.CreateFromSphere(turretBone.Meshes[0].BoundingSphere, turret.position, modelScale, this.rotationMatrix);

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
        private void SetPlayerKeys()
        {
            if (playerNumber == PlayerNumber.PlayerOne)
            {
                this.playerKeys = new PlayerKeys(Keys.W, Keys.S, Keys.A, Keys.D, Keys.Space,
                    Keys.O, Keys.Left, Keys.Right, Keys.Up, Keys.Down);
            }
            else if (playerNumber == PlayerNumber.PlayerTwo)
            {
                this.playerKeys = new PlayerKeys(Keys.I, Keys.K, Keys.J, Keys.L, Keys.U,
                    Keys.D0, Keys.N, Keys.M, Keys.Y, Keys.H);
            }
        }

        //Inputs
        private void Move(GameTime gameTime)
        {
            if (Input.IsPressedDown(playerKeys.Forward) && !Input.IsPressedDown(playerKeys.Backward))
            {
                this.position += this.relativeForward * (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.rightFrontWheelAngle += MathHelper.ToRadians(10f);
                this.leftFrontWheelAngle += MathHelper.ToRadians(10f);
                this.rightBackWheelAngle += MathHelper.ToRadians(10f);
                this.leftBackWheelAngle += MathHelper.ToRadians(10f);
            }
            else if (Input.IsPressedDown(playerKeys.Backward) && !Input.IsPressedDown(playerKeys.Forward))
            {
                this.position -= this.relativeForward * (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.rightFrontWheelAngle -= MathHelper.ToRadians(10f);
                this.leftFrontWheelAngle -= MathHelper.ToRadians(10f);
                this.rightBackWheelAngle -= MathHelper.ToRadians(10f);
                this.leftBackWheelAngle -= MathHelper.ToRadians(10f);
            }
            
        }
        private void Rotate(GameTime gameTime)
        {
            if (rotation.X > 2 * MathHelper.Pi || rotation.X < -2 * MathHelper.Pi)
                rotation.X = 0; //TODO: END: VER SE ISTO ESTA A FAZER ALGUMA COISA?

            #region wheels and movement
            if (Input.IsPressedDown(playerKeys.Left) && !Input.IsPressedDown(playerKeys.Right))
            {
                rightSteerAngle = MathHelper.ToRadians(10f);
                leftSteerAngle = MathHelper.ToRadians(10f);

                if (Input.IsPressedDown(playerKeys.Forward))
                    rotation.Y += MathHelper.ToRadians(1f);
                else if (Input.IsPressedDown(playerKeys.Backward))
                    rotation.Y -= MathHelper.ToRadians(1f);
            }
            else if (Input.IsPressedDown(playerKeys.Right) && !Input.IsPressedDown(playerKeys.Left))
            {
                rightSteerAngle = MathHelper.ToRadians(-10f);
                leftSteerAngle = MathHelper.ToRadians(-10f);

                if (Input.IsPressedDown(playerKeys.Forward))
                    rotation.Y -= MathHelper.ToRadians(1f);
                else if (Input.IsPressedDown(playerKeys.Backward))
                    rotation.Y += MathHelper.ToRadians(1f);

            }
            else
            {
                rightSteerAngle = MathHelper.ToRadians(0f);
                leftSteerAngle = MathHelper.ToRadians(0f);
            }
            #endregion

            #region canhao
            if (Input.IsPressedDown(playerKeys.CannonUp) && !Input.IsPressedDown(playerKeys.CannonDown))
            {
                if (this.cannon.rotation.X >= -Math.PI / 4)
                    this.cannon.rotation.X -= MathHelper.ToRadians(1f);
            }
            else if (Input.IsPressedDown(playerKeys.CannonDown) && !Input.IsPressedDown(playerKeys.CannonUp))
                if (this.cannon.rotation.X <= 0)
                    this.cannon.rotation.X += MathHelper.ToRadians(1f);
            #endregion

            #region torre
            if (Input.IsPressedDown(playerKeys.TurretLeft) && !Input.IsPressedDown(playerKeys.TurretRight))
                this.turret.rotation.Y += MathHelper.ToRadians(1f);
            else if (Input.IsPressedDown(playerKeys.TurretRight) && !Input.IsPressedDown(playerKeys.TurretLeft))
                this.turret.rotation.Y -= MathHelper.ToRadians(1f);
            #endregion
        }
        private void UpdateHatchet(GameTime gameTime)
        {
            if (Input.WasPressed(playerKeys.HatchetOpen))
                isOpenning = !isOpenning;

            if (hatchetAngle <= Math.PI / 2f && isOpenning)
                this.hatchetAngle += MathHelper.ToRadians(100f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (hatchetAngle >= 0 && !isOpenning)
                this.hatchetAngle -= MathHelper.ToRadians(100f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        private void Shoot()
        {
            bulletList.Add(new Bullet(cannon.position, cannon.Forward, cannon.Up));
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

        //--------------------Update&Draw--------------------//

        internal void Update(GameTime gameTime)
        {
            Move(gameTime);
            Rotate(gameTime);
            UpdateHatchet(gameTime);

            relativeForward = Vector3.Normalize(Vector3.Transform(Vector3.Forward, Matrix.CreateRotationY(rotation.Y)));
            relativeRight = Vector3.Normalize(Vector3.Transform(Vector3.Right, Matrix.CreateRotationY(rotation.Y)));

            HeightFollow();
            NormalFollow();

            translationMatrix = Matrix.CreateTranslation(position); ;
            rotationMatrix.Up = this.Up;
            //The 3d model is facing backwards.
            rotationMatrix.Forward = -this.Forward;
            rotationMatrix.Right = -this.Right;

            tankModel.Root.Transform = Matrix.CreateScale(modelScale) * rotationMatrix * translationMatrix;

            //Turret bone
            turret.Update(this.position, this.rotationMatrix);
            turretBone.Transform = turret.boneTransform;

            //Cannon bone
            cannon.Update(this.position, this.rotationMatrix, turret);
            cannonBone.Transform = cannon.boneTransform;

            rightSteerBone.Transform = Matrix.CreateRotationY(rightSteerAngle) * rightSteerTransform;
            leftSteerBone.Transform = Matrix.CreateRotationY(leftSteerAngle) * leftSteerTransform;
            rightFrontWheelBone.Transform = Matrix.CreateRotationX(rightFrontWheelAngle) * rightFrontWheelTransform;
            leftFrontWheelBone.Transform = Matrix.CreateRotationX(leftFrontWheelAngle) * leftFrontWheelTransform;
            rightBackWheelBone.Transform = Matrix.CreateRotationX(rightBackWheelAngle) * rightBackWheelTransform;
            leftBackWheelBone.Transform = Matrix.CreateRotationX(leftBackWheelAngle) * leftBackWheelTransform;
            hatchBone.Transform = Matrix.CreateRotationX(hatchetAngle) * hatchTransform;

            tankModel.CopyAbsoluteBoneTransformsTo(boneTransformations);
            boundingBox.Update(position, Forward, Right, Up);
            if (Input.WasPressed(playerKeys.Shoot))
            {
                Shoot();
            }

            for(int i = bulletList.Count - 1; i >= 0; i--)
            {
                bulletList[i].Update(gameTime);
                if (bulletList[i].position.Y <= 0 || OBB.AreColliding(bulletList[i].boundingBox, gameState.playerTwo.boundingBox))
                    bulletList.Remove(bulletList[i]);
            }
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
                }
                mesh.Draw();
            }

            foreach(Bullet b in bulletList)
            {
                b.Draw(cam);
            }
        }
    }
}
