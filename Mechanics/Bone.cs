using Microsoft.Xna.Framework;

namespace TankProject
{
    class Bone : GameObject
    {
        internal Matrix boneMatrix;
        internal Matrix originalBoneMatrix;

        internal Bone(Matrix boneMatrix, Vector3 objectPosition, float angle) : base(boneMatrix.Translation + objectPosition, new Vector3(angle, 0.0f, 0.0f), Vector3.Zero)
        {
            this.boneMatrix = originalBoneMatrix = boneMatrix;
            this.Forward = boneMatrix.Up;
            this.Right = boneMatrix.Right;
            this.Up = boneMatrix.Forward;
        }

        internal void Update()
        {
            this.Forward = Vector3.Normalize(boneMatrix.Forward);
            this.Right = Vector3.Normalize(boneMatrix.Right);
            this.Up = Vector3.Normalize(boneMatrix.Up);
        }
    }
}
