using Microsoft.Xna.Framework;

namespace TankProject
{
    class GameObject
    {
        internal Vector3 position;
        internal Vector3 rotation;
        internal Vector3 velocity;

        internal Vector3 relativeForward, relativeRight;
        internal Vector3 Up, Forward, Right;

        protected Matrix rotationMatrix;
        protected Matrix transformMatrix;

        //--------------------Constructors--------------------//

        internal GameObject(Vector3 position, Vector3 rotation, Vector3 velocity)
        {
            this.position = position;
            this.velocity = velocity;
            this.rotation = rotation;
            rotationMatrix = Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z);
            transformMatrix = Matrix.CreateTranslation(position);
        }
    }
}
