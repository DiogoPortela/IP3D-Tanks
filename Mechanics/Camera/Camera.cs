using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TankProject
{
    /// <summary>
    /// Base class for cameras. Do not edit.
    /// </summary>
    class Camera
    {
        internal Vector3 Position;

        internal Vector3 Forward;
        internal Vector3 Up;
        internal Vector3 Right;

        protected Vector3 relativeForward;
        protected Vector3 relativeRight;

        internal Matrix ViewMatrix;
        internal Matrix ProjectionMatrix;
        protected Matrix rotationMatrix;
        protected Vector3 rotation;

        protected float cameraSpeed;

        //--------------------Constructors--------------------//
        internal Camera(GraphicsDevice device, Vector3 position, float aspectRatio, float cameraSpeed = 5.0f, float fieldOfView = 45.0f)
        {
            this.Position = position;
            this.Forward = this.relativeForward = Vector3.Forward;
            this.Up = Vector3.Up;
            this.Right = this.relativeRight = Vector3.Right;
            this.cameraSpeed = cameraSpeed;
            this.rotation = Vector3.Zero;
            this.rotationMatrix = Matrix.Identity;

            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(fieldOfView), aspectRatio, 0.1f, 500.0f);
            ViewMatrix = Matrix.CreateLookAt(this.Position, this.Position + this.Forward, this.Up);
        }
        internal Camera(Camera camera)
        {
            this.Position = camera.Position;
            this.Forward = camera.Forward;
            this.relativeForward = camera.relativeForward;
            this.Up = camera.Up;
            this.Right = camera.Right;
            this.relativeRight = camera.relativeRight;
            this.cameraSpeed = camera.cameraSpeed;
            this.rotation = camera.rotation;
            this.rotationMatrix = camera.rotationMatrix;
            this.ProjectionMatrix = camera.ProjectionMatrix;
        }

        //--------------------Update&Draw--------------------//
        internal virtual void Update(GameTime gameTime)
        {
            rotation.Y = Math.Max(-1.3f, Math.Min(1.3f, rotation.Y));
            return;
        }
    }
}
