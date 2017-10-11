using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TankProject
{
    class CameraThirdPerson : Camera
    {
        GameObject target;
        Vector3 targetPosition;
        Vector3 targetToCameraVector;
        float distanceToTarget;

        internal CameraThirdPerson(GraphicsDevice device, Vector3 position, GameObject target, float distanceToTarget, float cameraSpeed = 5, float fieldOfView = 45) : base(device, position, cameraSpeed, fieldOfView)
        {
            this.target = target;
            this.distanceToTarget = distanceToTarget;
        }
        internal CameraThirdPerson(Camera camera, GameObject target, float distanceToTarget) : base (camera) //: base(camera.device, camera.Position, camera.ca, camera.fieldOfView)
        {
            this.target = target;
            this.distanceToTarget = distanceToTarget;
            this.Position = Vector3.Zero;
        }

        internal override void Update(GameTime gameTime)
        {
            this.targetPosition = target.position;
            this.Position = target.position + targetToCameraVector;
            Rotate(gameTime);
            this.ViewMatrix = Matrix.CreateLookAt(Position, target.position, Vector3.Up);
        }

        internal void Rotate(GameTime gameTime)
        {
            if (Input.xAxis != 0)
            {
                rotation.X += Input.xAxis * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (Input.yAxis != 0)
            {
                rotation.Y -= Input.yAxis * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (Input.MouseWheelValue() != 0)
            {
                distanceToTarget += (float)gameTime.ElapsedGameTime.TotalSeconds * Input.MouseWheelValue();
                if (distanceToTarget < 0.1f)
                    distanceToTarget = 0.1f;
            }
            targetToCameraVector = new Vector3(0, 0, distanceToTarget);
            rotationMatrix = Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, 0);
            targetToCameraVector = Vector3.Transform(targetToCameraVector, rotationMatrix);
            this.Forward = Vector3.Transform(Vector3.Forward, rotationMatrix);
            this.Right = Vector3.Transform(Vector3.Right, rotationMatrix);

        }
    }
}
