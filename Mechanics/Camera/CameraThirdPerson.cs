using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TankProject
{
    class CameraThirdPerson : Camera
    {
        GameObject target;
        Vector3 targetToCameraVector;
        float distanceToTarget;

        //--------------------Constructors--------------------//

        internal CameraThirdPerson(GraphicsDevice device, Vector3 position, GameObject target, float aspectRatio, float distanceToTarget, float cameraSpeed = 5.0f, float fieldOfView = 45.0f) : base(device, position, aspectRatio, cameraSpeed, fieldOfView)
        {
            this.target = target;
            this.distanceToTarget = distanceToTarget;
        }
        internal CameraThirdPerson(Camera camera, GameObject target, float distanceToTarget, GameTime gameTime) : base (camera)
        {
            this.target = target;
            this.distanceToTarget = distanceToTarget;
            Rotate(gameTime);

            this.Position = target.position + targetToCameraVector;
        }

        //--------------------Functions--------------------//

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
                distanceToTarget -= (float)gameTime.ElapsedGameTime.TotalSeconds * 0.1f * Input.MouseWheelValue();
                if (distanceToTarget < 0.1f)
                    distanceToTarget = 0.1f;
            }
            base.Update(gameTime);

            targetToCameraVector = new Vector3(0, 0, distanceToTarget);
            this.rotationMatrix = Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, 0);
            targetToCameraVector = Vector3.Transform(targetToCameraVector, rotationMatrix);
            this.Forward = Vector3.Transform(Vector3.Forward, rotationMatrix);
            this.Right = Vector3.Transform(Vector3.Right, rotationMatrix);
        }
        
        //--------------------Update&Draw--------------------//

        internal override void Update(GameTime gameTime)
        {
            this.Position = target.position + targetToCameraVector;
            Rotate(gameTime);
            this.ViewMatrix = Matrix.CreateLookAt(Position, target.position, Vector3.Up);
        }
    }
}
