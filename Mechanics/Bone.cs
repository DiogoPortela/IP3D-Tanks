using Microsoft.Xna.Framework;

namespace TankProject
{
    class Bone : GameObject
    {
        internal Matrix boneTransform;
        internal Vector3 bonePositionOffset;
        private float scale;

        internal Bone(Matrix boneMatrix, Vector3 objectPosition, Vector3 rotation, float scale) : base(boneMatrix.Translation * scale + objectPosition, rotation, Vector3.Zero)
        {
            this.boneTransform = boneMatrix;
            this.Forward = boneMatrix.Forward;
            this.Right = boneMatrix.Right;
            this.Up = boneMatrix.Up;
            this.scale = scale;
            bonePositionOffset = boneMatrix.Translation * scale;
            this.translationMatrix = Matrix.CreateTranslation(boneMatrix.Translation);
        }

        internal void Update(Vector3 position, Matrix objRotationMatrix)
        {
            this.rotationMatrix = Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z);
            this.boneTransform = this.rotationMatrix * this.translationMatrix;

            this.bonePositionOffset = Vector3.Transform(boneTransform.Translation * scale, objRotationMatrix);
            this.position = bonePositionOffset + position;

            this.Forward = -(boneTransform * objRotationMatrix).Forward;
            this.Right = -(boneTransform * objRotationMatrix).Right;
            this.Up = (boneTransform * objRotationMatrix).Up;
        }
        internal void Update(Vector3 position, Matrix objRotationMatrix, Bone previousBone)
        {
            this.rotationMatrix = Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z);
            this.boneTransform = this.rotationMatrix * this.translationMatrix;

            this.bonePositionOffset = Vector3.Transform(boneTransform.Translation * scale, previousBone.rotationMatrix * objRotationMatrix);
            this.position = bonePositionOffset + position;

            this.Forward = -(boneTransform * previousBone.rotationMatrix * objRotationMatrix).Forward;
            this.Right = -(boneTransform * previousBone.rotationMatrix * objRotationMatrix).Right;
            this.Up = (boneTransform * previousBone.rotationMatrix * objRotationMatrix).Up;
        }
    }
}
