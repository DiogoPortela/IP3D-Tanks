using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        private PlayerIndex playerIndex;

        internal float hp;
        internal int score;

        private PlayerKeys playerKeys;


        //test zone
        internal Vector3 lastFramePosition;
        private Vector3 aceleration;

        private ParticleSystem leftSmoke;
        private Vector3 leftSmokeOffset;
        private Vector3 initialLeftSmokeOffset;
        private ParticleSystem rightSmoke;
        private Vector3 rightSmokeOffset;
        private Vector3 initialRightSmokeOffset;

        private ParticleSystem leftDirt;
        private Vector3 leftDirtOffset;
        private Vector3 initialLeftDirtOffset;
        private ParticleSystem rightDirt;
        private Vector3 rightDirtOffset;
        private Vector3 initialRightDirtOffset;

        private SoundEffectInstance shootSoundFX;
        private SoundEffectInstance turnSoundFX;


        //--------------------Constructors--------------------//

        internal Player(Vector3 position, Vector3 rotation, Vector3 velocity, float modelScale, PlayerIndex index)
            : base(position, rotation, velocity, 100)
        {
            this.hp = base.hp;
            this.score = 0;
            this.relativeForward = this.Forward = Vector3.Forward;
            this.relativeRight = this.Right = Vector3.Right;
            this.Up = Vector3.Up;
            this.modelScale = modelScale;
            this.playerIndex = index;
            this.lastFramePosition = position;
            bulletList = new List<Bullet>();
            SetPlayerKeys();

            aceleration = Vector3.Zero;
            velocity = Vector3.Zero;

            initialLeftSmokeOffset = leftSmokeOffset = new Vector3(0.08f, 0.15f, -0.15f);
            initialRightSmokeOffset = rightSmokeOffset = new Vector3(-0.08f, 0.15f, -0.15f);
            initialLeftDirtOffset = leftDirtOffset = new Vector3(0.08f, 0.0f, -0.15f);
            initialRightDirtOffset = rightDirtOffset = new Vector3(-0.08f, 0.0f, -0.15f);
        }

        //--------------------Functions--------------------//

        //Loads
        internal void LoadModelBones(ContentManager content, Material material, Light light)
        {
            shootSoundFX = content.Load<SoundEffect>("pimba").CreateInstance();
            shootSoundFX.Volume = 0.5f;
            turnSoundFX = content.Load<SoundEffect>("girar_torres").CreateInstance();
            turnSoundFX.IsLooped = true;
            turnSoundFX.Volume = 1f;

            leftDirt = new ParticleSystem(ParticleType.Smoke, this.position + leftDirtOffset, new ParticleSpawner(0.01f, true), content, 250, 500, 5);
            rightDirt = new ParticleSystem(ParticleType.Smoke, this.position + rightDirtOffset, new ParticleSpawner(0.01f, true), content, 250, 500, 5);

            leftSmoke = new ParticleSystem(ParticleType.Smoke, this.position + leftSmokeOffset, new ParticleSpawner(0.01f, true), content, 250, 400, 5);
            rightSmoke = new ParticleSystem(ParticleType.Smoke, this.position + rightSmokeOffset, new ParticleSpawner(0.01f, true), content, 250, 400, 5);

            tankModel = content.Load<Model>("tank");
            boundingBox = OBB.CreateFromSphere(tankModel.Root.Meshes[0].BoundingSphere, this.position, modelScale, this.rotationMatrix);

            this.cannonBone = tankModel.Bones["canon_geo"];
            cannon = new Bone(cannonBone.Transform, this.position, Vector3.Zero, modelScale);
            cannon.boundingBox = OBB.CreateFromSphere(cannonBone.Meshes[0].BoundingSphere, cannon.position, modelScale, this.rotationMatrix);
            //TODO: FIX CANNON's BOUNDING BOX

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
            if (playerIndex == PlayerIndex.One)
            {
                this.playerKeys = new PlayerKeys(Keys.W, Keys.S, Keys.A, Keys.D, Keys.Space,
                    Keys.Q, Keys.F, Keys.H, Keys.T, Keys.G);
            }
            else if (playerIndex == PlayerIndex.Two)
            {
                this.playerKeys = new PlayerKeys(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.Enter,
                    Keys.O, Keys.J, Keys.L, Keys.I, Keys.K);
            }
        }

        //Inputs
        private void Move(GameTime gameTime)
        {
            if ((Input.IsPressedDown(playerKeys.Forward) && !Input.IsPressedDown(playerKeys.Backward)) ||
                (Input.IsPressedDown(Buttons.DPadUp, playerIndex) && !Input.IsPressedDown(Buttons.DPadDown, playerIndex)) ||
                (Input.IsPressedDown(Buttons.LeftThumbstickUp, playerIndex) && !Input.IsPressedDown(Buttons.LeftThumbstickDown, playerIndex)))
            {
                this.position += this.relativeForward * (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.rightFrontWheelAngle += MathHelper.ToRadians(10f);
                this.leftFrontWheelAngle += MathHelper.ToRadians(10f);
                this.rightBackWheelAngle += MathHelper.ToRadians(10f);
                this.leftBackWheelAngle += MathHelper.ToRadians(10f);
                leftDirt.SetShouldSpawn(true);
                rightDirt.SetShouldSpawn(true);
            }
            else if ((Input.IsPressedDown(playerKeys.Backward) && !Input.IsPressedDown(playerKeys.Forward)) ||
                     (Input.IsPressedDown(Buttons.DPadDown, playerIndex) && !Input.IsPressedDown(Buttons.DPadUp, playerIndex)) ||
                     (Input.IsPressedDown(Buttons.LeftThumbstickDown, playerIndex) && !Input.IsPressedDown(Buttons.LeftThumbstickUp, playerIndex)))
            {
                this.position -= this.relativeForward * (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.rightFrontWheelAngle -= MathHelper.ToRadians(10f);
                this.leftFrontWheelAngle -= MathHelper.ToRadians(10f);
                this.rightBackWheelAngle -= MathHelper.ToRadians(10f);
                this.leftBackWheelAngle -= MathHelper.ToRadians(10f);
                leftDirt.SetShouldSpawn(true);
                rightDirt.SetShouldSpawn(true);
            }

        }
        private void Rotate(GameTime gameTime)
        {
            #region wheels and movement
            if ((Input.IsPressedDown(playerKeys.Left) && !Input.IsPressedDown(playerKeys.Right)) ||
                (Input.IsPressedDown(Buttons.DPadLeft, playerIndex) && !Input.IsPressedDown(Buttons.DPadRight, playerIndex)) ||
                (Input.IsPressedDown(Buttons.LeftThumbstickLeft, playerIndex) && !Input.IsPressedDown(Buttons.LeftThumbstickRight, playerIndex)))
            {
                rightSteerAngle = MathHelper.ToRadians(10f);
                leftSteerAngle = MathHelper.ToRadians(10f);

                if (Input.IsPressedDown(playerKeys.Forward) ||
                    Input.IsPressedDown(Buttons.DPadUp, playerIndex) ||
                    Input.IsPressedDown(Buttons.LeftThumbstickUp, playerIndex))
                    rotation.Y += MathHelper.ToRadians(1f);
                else if (Input.IsPressedDown(playerKeys.Backward) ||
                         Input.IsPressedDown(Buttons.DPadDown, playerIndex) ||
                         Input.IsPressedDown(Buttons.LeftThumbstickDown, playerIndex))
                    rotation.Y -= MathHelper.ToRadians(1f);
            }
            else if ((Input.IsPressedDown(playerKeys.Right) && !Input.IsPressedDown(playerKeys.Left)) ||
                     (Input.IsPressedDown(Buttons.DPadRight, playerIndex) && !Input.IsPressedDown(Buttons.DPadLeft, playerIndex)) ||
                     (Input.IsPressedDown(Buttons.LeftThumbstickRight, playerIndex) && !Input.IsPressedDown(Buttons.LeftThumbstickLeft, playerIndex)))
            {
                rightSteerAngle = MathHelper.ToRadians(-10f);
                leftSteerAngle = MathHelper.ToRadians(-10f);

                if (Input.IsPressedDown(playerKeys.Forward) ||
                    Input.IsPressedDown(Buttons.DPadUp, playerIndex) ||
                    Input.IsPressedDown(Buttons.LeftThumbstickUp, playerIndex))
                    rotation.Y -= MathHelper.ToRadians(1f);
                else if (Input.IsPressedDown(playerKeys.Backward) ||
                         Input.IsPressedDown(Buttons.DPadDown, playerIndex) ||
                         Input.IsPressedDown(Buttons.LeftThumbstickDown, playerIndex))
                    rotation.Y += MathHelper.ToRadians(1f);

            }
            else
            {
                rightSteerAngle = MathHelper.ToRadians(0f);
                leftSteerAngle = MathHelper.ToRadians(0f);
            }
            #endregion

            #region canhao
            if ((Input.IsPressedDown(playerKeys.CannonUp) && !Input.IsPressedDown(playerKeys.CannonDown)) ||
                (Input.IsPressedDown(Buttons.RightThumbstickUp, playerIndex) && !Input.IsPressedDown(Buttons.RightThumbstickDown, playerIndex)))
            {
                if (this.cannon.rotation.X >= -Math.PI / 4)
                    this.cannon.rotation.X -= MathHelper.ToRadians(1f);
            }
            else if ((Input.IsPressedDown(playerKeys.CannonDown) && !Input.IsPressedDown(playerKeys.CannonUp)) ||
                     (Input.IsPressedDown(Buttons.RightThumbstickDown, playerIndex) && !Input.IsPressedDown(Buttons.RightThumbstickUp, playerIndex)))
                if (this.cannon.rotation.X <= 0)
                    this.cannon.rotation.X += MathHelper.ToRadians(1f);
            #endregion

            #region torre
            if ((Input.IsPressedDown(playerKeys.TurretLeft) && !Input.IsPressedDown(playerKeys.TurretRight)) ||
                (Input.IsPressedDown(Buttons.RightThumbstickLeft, playerIndex) && !Input.IsPressedDown(Buttons.RightThumbstickRight, playerIndex)))
            {
                turnSoundFX.Play();
                this.turret.rotation.Y += MathHelper.ToRadians(1f);
            }
            else if ((Input.IsPressedDown(playerKeys.TurretRight) && !Input.IsPressedDown(playerKeys.TurretLeft)) ||
                     (Input.IsPressedDown(Buttons.RightThumbstickRight, playerIndex) && !Input.IsPressedDown(Buttons.RightThumbstickLeft, playerIndex)))
            {
                turnSoundFX.Play();
                this.turret.rotation.Y -= MathHelper.ToRadians(1f);
            }
            else
                turnSoundFX.Stop();
            #endregion
        }
        private void UpdateHatchet(GameTime gameTime)
        {
            if (Input.WasPressed(playerKeys.HatchetOpen) ||
                Input.WasPressed(Buttons.Y, playerIndex))
                isOpenning = !isOpenning;

            if (hatchetAngle <= Math.PI / 2f && isOpenning)
                this.hatchetAngle += MathHelper.ToRadians(100f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (hatchetAngle >= 0 && !isOpenning)
                this.hatchetAngle -= MathHelper.ToRadians(100f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        private void Shoot()
        {
            if(shootSoundFX.State != SoundState.Playing)
            {
                bulletList.Add(new Bullet(cannon.position, cannon.Forward, cannon.Up));
                shootSoundFX.Play();
            }
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
        private void ClampInsideMap(ref Vector3 newPosition)
        {
            if (newPosition.X < 0)
                newPosition.X = 0;
            else if (newPosition.X > Floor.heightMap.Width - 1)
                newPosition.X = Floor.heightMap.Width - 1;
            if (newPosition.Z < 0)
                newPosition.Z = 0;
            else if (newPosition.Z > Floor.heightMap.Height - 1)
                newPosition.Z = Floor.heightMap.Height - 1;
        }

        //--------------------Update&Draw--------------------//

        internal void Update(GameTime gameTime)
        {
            leftDirt.SetShouldSpawn(false);
            rightDirt.SetShouldSpawn(false);
            lastFramePosition = position;
            Move(gameTime);
            Rotate(gameTime);
            UpdateHatchet(gameTime);

            relativeForward = Vector3.Normalize(Vector3.Transform(Vector3.Forward, Matrix.CreateRotationY(rotation.Y)));
            relativeRight = Vector3.Normalize(Vector3.Transform(Vector3.Right, Matrix.CreateRotationY(rotation.Y)));

            ClampInsideMap(ref position);

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
            if (Input.WasPressed(playerKeys.Shoot) ||
                Input.WasPressed(Buttons.RightTrigger, playerIndex) ||
                Input.WasPressed(Buttons.LeftTrigger, playerIndex))
            {
                Shoot();
            }

            foreach(Bullet b in bulletList)
            {
                b.Update(gameTime);
            }

            leftSmokeOffset = Vector3.Transform(initialLeftSmokeOffset, rotationMatrix);
            leftSmoke.Update(this.position + leftSmokeOffset, gameTime);
            rightSmokeOffset = Vector3.Transform(initialRightSmokeOffset, rotationMatrix);
            rightSmoke.Update(this.position + rightSmokeOffset, gameTime);

            leftDirtOffset = Vector3.Transform(initialLeftDirtOffset, rotationMatrix);
            leftDirt.Update(this.position + leftDirtOffset, gameTime);
            rightDirtOffset = Vector3.Transform(initialRightDirtOffset, rotationMatrix);
            rightDirt.Update(this.position + rightDirtOffset, gameTime);

        }
        internal void Draw(GraphicsDevice device, Camera cam)
        {
            foreach (ModelMesh mesh in tankModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = boneTransformations[mesh.ParentBone.Index];
                    effect.View = cam.ViewMatrix;
                    effect.Projection = cam.ProjectionMatrix;
                    effect.CurrentTechnique.Passes[0].Apply();
                }
                mesh.Draw();
            }

            foreach (Bullet b in bulletList)
            {
                b.Draw(cam);
            }
            leftSmoke.Draw(device, cam);
            rightSmoke.Draw(device, cam);
            leftDirt.Draw(device, cam);
            rightDirt.Draw(device, cam);
        }
    }
}
