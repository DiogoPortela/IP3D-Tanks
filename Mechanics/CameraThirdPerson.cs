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

        internal override void Update(GameTime gameTime)
        {
            this.targetPosition = target.position;
            Rotate(gameTime);
            this.ViewMatrix = Matrix.CreateLookAt(target.position + targetToCameraVector, target.position, Vector3.Up);
        }

        internal void Rotate(GameTime gameTime)
        {
            if (Input.xAxis != 0)
            {
                rotation.X += Input.xAxis * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (Input.yAxis != 0)
            {
                rotation.Y += Input.yAxis * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (Input.MouseWheelValue() != 0)
            {
                distanceToTarget += Input.MouseWheelValue();
                if (distanceToTarget < 0.1f)
                    distanceToTarget = 0.1f;
            }
            targetToCameraVector = new Vector3(0, distanceToTarget, 0);
            rotationMatrix = Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, 0);
            targetToCameraVector = Vector3.Transform(targetToCameraVector, rotationMatrix);
        }
    }
}
