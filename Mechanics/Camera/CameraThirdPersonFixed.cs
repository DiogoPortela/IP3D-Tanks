using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TankProject
{
    class CameraThirdPersonFixed : Camera
    {
        GameObject target;
        Vector3 lookDirection;
        Vector3 initialLookDirection;
        Vector3 lookPoint;
        Vector3 initialLookPoint;
        float distanceToTarget;

        internal CameraThirdPersonFixed(GraphicsDevice device, Vector3 position, GameObject target, float distanceToTarget, Vector3 lookDirection, Vector3 lookPoint, float aspectRatio, float cameraSpeed = 5, float fieldOfView = 45) : base(device, position, aspectRatio, cameraSpeed, fieldOfView)
        {
            this.target = target;
            this.distanceToTarget = distanceToTarget;
            this.initialLookDirection = this.lookDirection = lookDirection;
            this.initialLookPoint = this.lookPoint = lookPoint;
        }
        internal CameraThirdPersonFixed(Camera camera, GameObject target, float distanceToTarget, Vector3 lookDirection, Vector3 lookPoint) : base(camera)
        {
            this.target = target;
            this.distanceToTarget = distanceToTarget;
            this.initialLookDirection = this.lookDirection = lookDirection;
            this.initialLookPoint = this.lookPoint = lookPoint;
        }

        internal override void Update(GameTime gameTime)
        {
            rotationMatrix.Up = target.Up;
            rotationMatrix.Right = target.Right;
            rotationMatrix.Forward = target.Forward;
            rotation = target.rotation;
            lookDirection = Vector3.Transform(initialLookDirection, rotationMatrix);
            lookPoint = Vector3.Transform(initialLookPoint, rotationMatrix);
            this.Position = target.position + lookPoint + lookDirection;
            this.ViewMatrix = Matrix.CreateLookAt(Position, target.position + lookPoint, Vector3.Up);
        }
    }
}
