using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TankProject
{
    class CameraThirdPerson : Camera
    {
        GameObject target;
        Vector3 targetPosition;

        Matrix transformationMatrix;
        float distanceToTarget;

        internal CameraThirdPerson(GraphicsDevice device, Vector3 position, GameObject target, float distanceToTarget, float cameraSpeed = 5, float fieldOfView = 45) : base(device, position, cameraSpeed, fieldOfView)
        {
            this.target = target;
            this.distanceToTarget = distanceToTarget;
        }

        internal override void Update(GameTime gameTime)
        {
            targetPosition = target.position;
            base.Update(gameTime);
        }

        internal void Rotate(GameTime gameTime)
        {
            if (Input.xAxis != 0)
            {
                rotation.Z += Input.xAxis * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (Input.yAxis != 0)
            {
                rotation.X += Input.xAxis * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (Input.MouseWheelValue() != 0)
            {
                distanceToTarget += Input.MouseWheelValue();
            }
            rotationMatrix = Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Z, 0);
        }
    }
}
