using Microsoft.Xna.Framework;

namespace TankProject
{
    class Bone : GameObject
    {
        internal Matrix boneTransform;
        internal Vector3 bonePositionOffset;
        private float scale;

        //--------------------Constructors--------------------//
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

        //--------------------Update&Draw--------------------//
        internal void Update(Vector3 position, Matrix objRotationMatrix)
        {
            this.rotationMatrix = Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z);
            this.boneTransform = this.rotationMatrix * this.translationMatrix;

            this.bonePositionOffset = Vector3.Transform(boneTransform.Translation * scale, objRotationMatrix);
            this.position = position + bonePositionOffset;

            Matrix aux = this.rotationMatrix * objRotationMatrix * this.translationMatrix;
            this.Forward = -aux.Forward;
            this.Right = -aux.Right;
            this.Up = aux.Up;
        }
        internal void Update(Vector3 position, Matrix objRotationMatrix, Bone previousBone)
        {
            this.rotationMatrix = Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z);
            this.boneTransform = this.rotationMatrix * this.translationMatrix;

            this.bonePositionOffset = Vector3.Transform(translationMatrix.Translation * scale, previousBone.rotationMatrix * objRotationMatrix) + previousBone.bonePositionOffset;
            this.position = position + bonePositionOffset;

            Matrix aux = this.rotationMatrix * previousBone.rotationMatrix * objRotationMatrix * this.translationMatrix;
            this.Forward = -aux.Forward;
            this.Right = -aux.Right;
            this.Up = aux.Up;
        }
    }
}
