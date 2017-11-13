using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TankProject
{
    /// <summary>
    /// Free following camera. Use numpad to control.
    /// </summary>
    class CameraFree : Camera
    {
        //--------------------Constructors--------------------//
        internal CameraFree(GraphicsDevice device, Vector3 position, float aspectRatio, float cameraSpeed = 5.0f, float fieldOfView = 45.0f)
            : base(device, position, aspectRatio, cameraSpeed, fieldOfView)
        {

        }
        internal CameraFree(Camera camera) : base(camera)
        {

        }

        //--------------------Functions--------------------//

        internal virtual void Move(GameTime gameTime)
        {
            if (Input.IsPressedDown(Keys.NumPad8) && !Input.IsPressedDown(Keys.NumPad5))
                Position += Forward * cameraSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (Input.IsPressedDown(Keys.NumPad5) && !Input.IsPressedDown(Keys.NumPad8))
                Position -= Forward * cameraSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Input.IsPressedDown(Keys.NumPad4) && !Input.IsPressedDown(Keys.NumPad6))
                Position -= Right * cameraSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (Input.IsPressedDown(Keys.NumPad6) && !Input.IsPressedDown(Keys.NumPad4))
                Position += Right * cameraSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Input.IsPressedDown(Keys.NumPad7) && !Input.IsPressedDown(Keys.NumPad1))
                Position += Vector3.UnitY * cameraSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (Input.IsPressedDown(Keys.NumPad1) && !Input.IsPressedDown(Keys.NumPad7))
                Position -= Vector3.UnitY * cameraSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        internal virtual void Rotate(GameTime gameTime)
        {
            if (Input.xAxis != 0)
            {
                rotation.X -= (float)gameTime.ElapsedGameTime.TotalSeconds * Input.xAxis;
            }
            if (Input.yAxis != 0)
            {
                rotation.Y -= (float)gameTime.ElapsedGameTime.TotalSeconds * Input.yAxis;
            }

            base.Update(gameTime);

            this.rotationMatrix = Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, 0);
            this.Forward = Vector3.Transform(Vector3.Forward, rotationMatrix);
            this.Right = Vector3.Transform(Vector3.Right, rotationMatrix);
            this.Up = Vector3.Transform(Vector3.Up, rotationMatrix);
            this.relativeForward = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationY(rotation.X));
            this.relativeRight = Vector3.Transform(Vector3.Right, Matrix.CreateRotationY(rotation.X));
        }

        //--------------------Update&Draw--------------------//

        internal override void Update(GameTime gameTime)
        {
            Move(gameTime);
            Rotate(gameTime);
            ViewMatrix = Matrix.CreateLookAt(Position, Position + Forward, Up);
        }

    }
}
